using UnityEngine;

public class CylinderEnemy : Enemy {

    public override EnemyStyle Enemy_style
    {
        get { return EnemyStyle.Cylinder; }
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
