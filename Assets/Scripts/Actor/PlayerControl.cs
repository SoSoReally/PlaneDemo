using System;
using UnityEngine;
using DG.Tweening;
[DisallowMultipleComponent]
public class PlayerControl : ActorBase {

    public Mapping m_input_mapping;

    public float speed= 1f;

    public bool IsDeath {private set; get; }

    public override ActorEnums Actor_Enums
    {
        get
        {
            actor_Enums.actor = EnumActor.Player;

            return actor_Enums;
        }
    }   

    private InputManager.InputSystem m_input;

    [SerializeField]
    private MaterialMotion m_material_motion;

    private Snap snap;

    public Snap m_size= new Snap() { width = 0.8f,height =0.8f};

    void Start () {
        GetWeapon();

        m_material_motion.material = GetComponent<Renderer>().material ;



        OnHurt += OnHurtFlash;

        gameObject.layer = LayerCustom.Player;

        Timer.OnStep += Attack;

        m_input = GameManager.Get.Input_system;

        snap = GameManager.Get.CameraSnap(Camera.main);

        snap.width = Camera.main.ScreenToWorldPoint(new Vector3(snap.width, 0f, 20f)).x;

        snap.height = Camera.main.ScreenToWorldPoint(new Vector3(0f, snap.height, 20f)).y;

        Debug.Log("rect:" + snap.rect + "width" + snap.width + "hegith" + snap.height + "pixelRect" + snap.pixelRect);
    }

	void Update () {
        InputControl();
	}


    private void OnTriggerEnter(Collider other)
    {
        int layer = other.gameObject.layer;

        if (layer ==LayerCustom.EnemyBullet ||layer == LayerCustom.EnemyBulletCanDestory)
        {
            var weapon= other.GetComponent<Weapon>();

            Hurt(weapon.Attack_value+weapon.Source_attack_value);

        }
        if (layer == LayerCustom.Enemy)
        {
            var action = other.GetComponent<ActorBase>();

            Hurt(action.Attack_value);
        }
    }

    protected override void Move()
    {
        if (m_input.Attack())
        {
            Timer.Step = m_weapon.Timer.Step;

            Timer.Update(Timer);
        }

        Vector2 move_dir = m_input.Move();

        float c = 0f;

        if (move_dir.magnitude > 0f)
        {
            c = Ex.RotationDir2D(Vector3.right, move_dir);

            move_dir = Ex.Vec2GetDirForAngle(Vector3.Angle(move_dir, Vector3.right) * c);

            UpdatMove(move_dir * speed * Time.deltaTime, Space.World);
        }
    }

    private void Rotation()
    {
        Vector2 rotation_dir = m_input.Rotation();

        float c = 0f;

        if (rotation_dir.magnitude<=0f)
        {
            return;
        }

        if (!m_input.State.IsConnected)
        {
            Vector2 vec2 = Camera.main.WorldToScreenPoint(transform.position);

            rotation_dir =  rotation_dir -vec2;

            transform.UPLookAt(rotation_dir.normalized);
        }
        else
        {
            var lerpAngle = Vector3.Angle(rotation_dir, transform.up);

            float angle = Vector3.Angle(rotation_dir, Vector3.up);

            c = Ex.RotationDir2D(Vector3.up, rotation_dir);

            float step = 180f * Time.deltaTime;

            Quaternion a = new Quaternion() { eulerAngles = new Vector3(0f, 0f, angle * c) };

            transform.rotation = Quaternion.Slerp(transform.rotation, a, (step / lerpAngle));

            lerpAngle = Vector3.Angle(rotation_dir, transform.up);
        }
    }

    private void UpdatMove(Vector3 vec3 , Space space)
    {
        var ptr_vec3  = IsOutofbounds(vec3);

        transform.Translate(ptr_vec3, space);
    }

    private Vector3 IsOutofbounds(Vector3 vec3)
    {
        Vector3 pos = vec3 + transform.position;

        if (pos.x > snap.width - m_size.width / 2f || pos.x < m_size.width / 2f - snap.width)
        {
            vec3.x = 0f;
        }
        if (pos.y>snap.height-m_size.height/2f||pos.y< -snap.height+m_size.height/2f)
        {
            vec3.y = 0f;
        }

        vec3.z = 0f;

        return vec3;
    }

    void InputControl()
    {
        Move();
        Rotation();
    }

    protected override void Hurt(int value)
    {
        if (m_material_motion.AlphaState)
        {
            return;
        }

        HurtAction(value);

        Life_value -= value;

       // Debug.Log(Life_value + " palyer " + value);

        if (Life_value <= 0)
        {
            // Debug.Log("Game over");
            if (!IsDeath)
            {
                Dealth();

                IsDeath = true;
            }
         
        }

        m_material_motion.AlphaState = true;
    }

    protected override void Attack()
    {
        var bullet = GameManager.Get.Spawn_system.OnSpawn(m_weapon.GetComponent<SpawnID>());

        var weapon = bullet.GetComponent<Weapon>();

        weapon.Init(transform.root, transform.up + transform.position, LayerCustom.PlayerBullet, Attack_value, transform.up, ActorMotionMode.Line);

        AttackAction();

        GameManager.Get.Sound_system.Current.PlayOneShot(AudioGet.Enum(weapon.Enum_weapon));
    }

    public override void Dealth()
    {

        GameManager.Get.Game_over += ResetPlay;

        GameManager.Get.GameOver();

    }

    private void GetWeapon()
    {
        m_weapon = GameManager.Get.Weapon_system.GetWeapon(WeaponGet.Enum(Actor_Enums.actor, Actor_Enums.weapon));
    }

    private void OnHurtFlash(int index)
    {
        var m = m_material_motion;

        m.material.DOFloat(1f, m.alpha, m.duration).SetEase(m.ac).ChangeStartValue(0f).OnComplete(() => { m.AlphaState = false; });
    }

    public void ResetPlay()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }
    [Serializable]
    private class  MaterialMotion{
        [HideInInspector]
        public Material material;

        [HideInInspector]
        public readonly string alpha = "_Alpha";

        public AnimationCurve ac = null;

        public float duration=1f;

        public bool AlphaState { set; get; }
    }
}
