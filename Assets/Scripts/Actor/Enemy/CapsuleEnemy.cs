using UnityEngine;
public class CapsuleEnemy : Enemy {

    public override EnemyStyle Enemy_style
    {
        get { return EnemyStyle.Capsule; }
    }

    GameObject dir_obj;
    // Use this for initialization
    void Start () {

        OnStart();

        Timer.OnStep += Attack;

        CreatDirObj();
    }
	
	// Update is called once per frame
	void Update () {
        Timer.Update(Timer);
        Move(); 
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerCustom.PlayerBullet)
        {
            var weapon = other.GetComponent<Weapon>();
            Hurt(weapon.Attack_value+weapon.Source_attack_value);
        }
    }

    void CreatDirObj()
    {
        dir_obj = new GameObject();
        dir_obj.transform.SetParent(transform);
        dir_obj.transform.localPosition = Vector3.zero;
        dir_obj.transform.rotation = Quaternion.identity;
    }

    protected override void Attack()
    {
        if (!IsAttacking)
        {
            return;
        }
        
        dir_obj.transform.Rotate(dir_obj.transform.forward, 10f,Space.World);
        for (int i = 0; i < emit_point; i++)
        {
            dir_obj.transform.Rotate(dir_obj.transform.forward, 360/emit_point,Space.World);

            Weapon spawn = GameManager.Get.Spawn_system.OnSpawn(m_weapon.Spawn_ID).GetComponent<Weapon>();

            Vector3 dir = dir_obj.transform.right;

            spawn.Init(null, transform.position + dir, LayerCustom.EnemyBulletCanDestory, Attack_value, dir, ActorMotionMode.Line);

            spawn.InitActorMotion(weapon_motion.start_speed,weapon_motion.ease,weapon_motion.end_speed);

            GameManager.Get.Spawn_system.WaitRecovery(spawn.Spawn_ID);
            
       }
        AttackAction();
    }

    protected override void Hurt(int value)
    {
        base.Hurt(value);
    }

    protected override void Move()
    {
        base.Move();
    }
}
