using DG.Tweening;
using System;
using UnityEngine;

public class CubeEnemy : Enemy {
    
    
    public override EnemyStyle Enemy_style
    {
        get
        {
            return EnemyStyle.Cube;
        }
    }

    [SerializeField]
    private MaterialMotion m;

    void Start()
    {
        OnStart();

        Timer.OnStep += Attack;

        Emit_point = 4;

        m.Material = GetComponent<Renderer>().material;

        m.Start = m.Material.GetColor(m.name);

        m.max = Life_value;

        OnHurt += OnHurtChangColor;

      //  m.tween= transform.DORotate(new Vector3(0f,0f,180f), 2f, RotateMode.WorldAxisAdd).SetLoops(int.MaxValue);
    }

    void Update()
    {
        if (m.isAlpha)
        {
            return;
        }

        Move();

        Timer.Update(Timer);
    }

   
    protected override void Attack()
    {
        if (!IsAttacking)
        {
            return;
        }
        Vector3 dir = new Vector3();
        for (int i = 0; i < Emit_point; i++)
        {
            float z = transform.localEulerAngles.z;

            dir = Ex.Vec3GetDirForAngle(i*90f +z);

            var weapon=  GameManager.Get.Spawn_system.OnSpawn(m_weapon.Spawn_ID).GetComponent<Weapon>();

            weapon.Init(GameManager.Get.Player.transform, dir + transform.position, LayerCustom.EnemyBullet, 0, new Vector3(0f, 0f, i * 90f + z - 90f), ActorMotionMode.Tracker);

            //dir = Vector3.Normalize(GameManager.Get.Player.transform.position - transform.position);

        }
    }

    protected override void Hurt(int value)
    {
        if (m.isAlpha)
        {
            return;
        }

        Life_value -= value;

        HurtAction(value);

        if (Life_value <= 0)
        {
            Dealth();

            m.isAlpha = true;

            // m.tween.Pause();

            DOVirtual.DelayedCall(0.12f, Clear);
           
        }
    }

    protected override void Move()
    {
        base.Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerCustom.PlayerBullet)
        {
            var weapon = other.GetComponent<Weapon>();

            Hurt(weapon.Attack_value + weapon.Source_attack_value);
        }
    }

    private void OnHurtChangColor(int value)
    {
        float lerp = Life_value / m.max;

        Color set = Color.Lerp(m.target, m.Start, lerp);

        m.Material.SetColor(m.name, set);
    }

    public override void Dealth()
    {
        GameManager.Get.Sound_system.Current.PlayOneShot(AudioGet.Enum(DealthAudio));

        GameManager.Get.Particle_system.PlayOnPos(DealthParticle, transform.position,transform.localEulerAngles);
    }

    public override void ResetValue()
    {
        var source = GameManager.Get.Spawn_system.GetType<Enemy>(spawn_id);

        m.Material.CopyPropertiesFromMaterial(source.GetComponent<Renderer>().sharedMaterial);

        Life_value = source.Life_value;

        OnHurt += OnHurtChangColor;

        m.isAlpha = false;
    }

    [Serializable]
    private class MaterialMotion
    {
        public Material Material { set; get; }

        public Color Start { set; get; }

        public Color target=Color.white;

        [HideInInspector]
        public float max;

        public readonly string name = "_TintColor";

        [HideInInspector]
        public bool isAlpha = false;
    }
}
