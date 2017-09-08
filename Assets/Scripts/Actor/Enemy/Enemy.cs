using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Animation)), RequireComponent(typeof(SpawnID)),DisallowMultipleComponent]
public abstract class Enemy : ActorBase, ActorMotion.IMotion, ISpawnID
{
    [SerializeField]
    protected int emit_point;

    protected void Awake()
    {
        var id = Spawn_ID;
    }

    public int Emit_point
    {
        get
        {
            return emit_point > 0 ? emit_point : 0;
        }

        set
        {
            emit_point = value > 0 ? value : 0;
        }
    }

    private bool m_isAttacking = false;

    public bool IsAttacking
    {
        get
        {
            return m_isAttacking;
        }
    }

    [SerializeField]
    protected ActorMotion actor_motion;

    public virtual ActorMotion Actor_motion
    {
        get
        {
            if (actor_motion == null)
            {
                actor_motion = new ActorMotion() ;
            }
            return actor_motion;
        }

        set
        {
            actor_motion = value;
            actor_motion.start_speed = value.start_speed > 0f ? value.start_speed : 0f;
        }
    }

    public ActorMotion weapon_motion;

    protected SpawnID spawn_id;

    public virtual SpawnID Spawn_ID
    {
        get
        {
            if (spawn_id == null)
            {
                spawn_id = GetComponent<SpawnID>();
            }
            if (spawn_id!=null)
            {
                if (spawn_id.Type == null)
                {
                    spawn_id.Type = this;
                }
            }
           
            return spawn_id;
        }
        set { spawn_id = value; }
    }

    protected Action ClearAction = new Action(() => { });

    public virtual event Action OnClear
    {
        add
        {
            if (!ClearAction.GetInvocationList().Contains(value))
            {
                ClearAction += value;
            }
        }
        remove { ClearAction -= value; }
    }

    public override ActorEnums Actor_Enums
    {
        get
        {
            actor_Enums.actor = EnumActor.Enemy;
            return actor_Enums;
        }
    }

    public abstract EnemyStyle Enemy_style { get; }

    protected Action<ActorMotion> ActorMotionAction = new Action<ActorMotion>((ActorMotion am)=>{});

    public virtual event Action<ActorMotion> OnActorMotion
    {
        add
        {
            if (ActorMotionAction.GetInvocationList().Contains(value))
            {
                ActorMotionAction += value;
            }
        }
        remove
        {
            ActorMotionAction -= value;
        }
    }

    public  virtual void Clear()
    {
        ClearAction();
        GameManager.Get.Spawn_system.RecoverySpawn(Spawn_ID);
        HurtAction = new Action<int>((int i) => { });
        AttackAction = new Action(() => { });
        ActorMotionAction = new Action<ActorMotion>((ActorMotion am) => { });
        ClearAction = new Action(() => { });
        ResetValue();
        gameObject.SetActive(false);
    }

    public virtual void Init(Vector3 startPos, Vector3? dir=null,Action<ActorMotion> action =null)
    {
        transform.position = startPos;

        ActorMotionAction += action;

        Vector3  newdir = dir==null? Vector3.zero:(Vector3)dir;

        Actor_motion.dir = newdir;

    }

    public virtual void InitActorMotion(float startSpeed,  Ease ease = Ease.Unset, float endSpeed = 0f, ActorMotion.UpdateMode updateMode = ActorMotion.UpdateMode.Update)
    {
        Actor_motion.start_speed = startSpeed;

        Actor_motion.ease = ease;

        Actor_motion.end_speed = endSpeed;

        Actor_motion.update_mode = updateMode;

       
    }

    public virtual void InitTimer(float stepTime, float overTime = 10f)
    {
        Timer.Overtime = overTime;

        Timer.Step = stepTime;

    }

    protected override void Hurt(int value)
    {
        Life_value -= value;
        HurtAction(value);
        if (Life_value <= 0)
        {
            Dealth();
            Clear();
        }
    }

    protected override void Move()
    {
        ActorMotionAction(actor_motion);
    }

    protected virtual void OnStart()
    {
        m_weapon = GameManager.Get.Weapon_system.GetWeapon(WeaponGet.Enum(Actor_Enums.actor, Actor_Enums.weapon));

        actor_motion.transform = transform;

        gameObject.layer = LayerCustom.Enemy;
    }

    public virtual void ResetValue()
    {
        var source = GameManager.Get.Spawn_system.GetType<Enemy>(spawn_id);

        Life_value = source.Life_value;

    }

    protected void OnBecameInvisible()
    {
        m_isAttacking = false;
    }

    protected void OnBecameVisible()
    {
        m_isAttacking = true;
    }

}

