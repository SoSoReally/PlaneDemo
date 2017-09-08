using UnityEngine;

public class SphereEnemy : Enemy {

    public override EnemyStyle Enemy_style
    {
        get { return EnemyStyle.Sphere; }
    }

    // Use this for initialization
    void Start()
    {
        OnStart();

        Timer.OnStep += Attack;
    }

    // Update is called once per frame
    void Update()
    {
        Timer.Update(Timer);
        Move();
        if (IsAttacking)
        {
            Vector3 dir = Vector3.Normalize(GameManager.Get.Player.transform.position - transform.position);

            transform.UPLookAt(dir);
        }
      
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerCustom.PlayerBullet)
        {
            var weapon = other.GetComponent<Weapon>();

            Hurt(weapon.Attack_value + weapon.Source_attack_value);
        }
       
    }

    protected override void Attack()
    {
        if (!IsAttacking)
        {
            return;
        }

        var weapon = GameManager.Get.Spawn_system.OnSpawn(m_weapon.Spawn_ID).GetComponent<Weapon>();

        Vector3 dir =Vector3.Normalize( GameManager.Get.Player.transform.position - transform.position);

        weapon.Init(null, transform.position + dir, LayerCustom.EnemyBulletCanDestory, Attack_value, dir, ActorMotionMode.Line);

        weapon.InitActorMotion(20f);

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

    public override void Clear()
    {
        base.Clear();
    }

}
