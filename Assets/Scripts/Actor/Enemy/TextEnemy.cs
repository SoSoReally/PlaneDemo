using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemy : Enemy {
    public override EnemyStyle Enemy_style
    {
        get
        {
            return EnemyStyle.Text;
        }
    }

    private Vector2 size = new Vector2(4.5f, 1f);

    private Vector2[] attackpoints = new Vector2[12];

    private Vector2[][] attackground = new Vector2[3][];

    private Weapon m_2_weapon;

    private Weapon m_3_weapon;

    private Timer timer_2 = new Timer();

    private Timer timer_3 = new Timer();

   

    void Start()
    {

        OnStart();

        Timer.OnStep += Attack;

        attackpoints[0] = new Vector2(size.x, 0f);

        InitAttackPoint();

        m_2_weapon = GameManager.Get.Weapon_system.GetWeapon(WeaponGet.Enum(EnumActor.Enemy, EnumWeapon.Default));

        m_3_weapon = GameManager.Get.Weapon_system.GetWeapon(WeaponGet.Enum(EnumActor.Enemy, EnumWeapon.Cone));

        timer_2.Step = 0.5f;

        timer_2.OnStep += Attack_2;

        timer_3.Step = 1.0f;

        timer_3.OnStep += Attack_3;
    }

    // Update is called once per frame
    void Update()
    {
        Timer.Update(Timer);

        Timer.Update(timer_2);

        Timer.Update(timer_3);

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

    private void InitAttackPoint()
    {

        float line = (size.x * 2f) / 4f;

        for (int i = 1; i < 6; i++)
        {
            attackpoints[i] = new Vector2(size.x - line * (i - 1), size.y);
        }

        attackpoints[6] = new Vector2(-size.x, 0f);

        for (int i = 7; i < 12; i++)
        {
            attackpoints[i] = new Vector2(line * (i - 7) - size.x, -size.y);
        }
        //foreach (var item in attackpoints)
        //{
        //    Debug.Log(item);
        //}

        attackground[0] = new Vector2[4] { attackpoints[0], attackpoints[3], attackpoints[6], attackpoints[9] };

        attackground[1] = new Vector2[4] { attackpoints[1], attackpoints[5], attackpoints[7], attackpoints[11] };

        attackground[2] = new Vector2[4] { attackpoints[2], attackpoints[4], attackpoints[8], attackpoints[10] };
    }

    protected override void Attack()
    {
        if (!IsAttacking)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            var weapon = GameManager.Get.Spawn_system.OnSpawn(m_weapon.Spawn_ID).GetComponent<Weapon>();

            Vector3 dir = attackground[0][i];

            weapon.Init(null, transform.position + dir, LayerCustom.EnemyBulletCanDestory , Attack_value, dir.normalized, ActorMotionMode.Line);

            weapon.InitActorMotion(20f);
        }

        AttackAction();
    }

    private void Attack_2()
    {
        if (!IsAttacking)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            var weapon = GameManager.Get.Spawn_system.OnSpawn(m_2_weapon.Spawn_ID).GetComponent<Weapon>();

            Vector3 pos = attackground[1][i];
            Vector3 dir = pos;
            dir.x /= size.x;

            weapon.Init(null, transform.position + dir, LayerCustom.EnemyBulletCanDestory, Attack_value,dir.normalized , ActorMotionMode.Line);

            weapon.InitActorMotion(20f);
        }
    }

    private void Attack_3()
    {
        if (!IsAttacking)
        {
            return;
        }
        for (int i = 0; i < 4; i++)
        {
            Vector3 pos = attackground[2][i];

            Vector3 dir = pos;

            var weapon = GameManager.Get.Spawn_system.OnSpawn(m_3_weapon.Spawn_ID).GetComponent<Weapon>();

            float z = Vector3.Angle(dir, transform.right);

            weapon.Init(GameManager.Get.Player.transform, dir + transform.position, LayerCustom.EnemyBullet, 0,new Vector3(0f,0f,z-90f), ActorMotionMode.Tracker);

        }
    }

    protected override void Hurt(int value)
    {
        base.Hurt(value);
    }

    protected override void Move()
    {
        base.Move();
    }

    public override void Dealth()
    {
        base.Dealth();

        GameManager.Get.GameOver();

        GameManager.Get.Game_over += () => {

            UnityEngine.SceneManagement.SceneManager.LoadScene(1);

            GameManager.Get.UI.GetComponent<ShowPanels>().ShowMenu();

            GameManager.Get.UI.GetComponent<StartOptions>().inMainMenu = true;
        };

        GameManager.Get.GameProgress_system.ProgressOver = true;
    }

}
