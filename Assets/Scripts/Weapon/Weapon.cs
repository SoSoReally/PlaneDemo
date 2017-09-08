using DG.Tweening;
using System;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SpawnID))]
public abstract class Weapon : MonoBehaviour, ActorMotion.IMotion, ISpawnID
{
    public abstract EnumWeapon Enum_weapon{ get; }

    [SerializeField]
    private Timer timer;

    public Timer Timer
    {
        get
        {
            if (timer ==null)
            {
                timer = new Timer();
            }
            return timer;
        }
    }

    protected Action ClearAction = new Action(() => { });

    public virtual event Action  OnClear {
        add {
            if (!ClearAction.GetInvocationList().Contains(value))
            {
                ClearAction += value;
            }
        }
        remove { ClearAction -= value; }
    }

    protected Action<ActorMotion> ActorMotionAction =new Action<ActorMotion>((ActorMotion am)=> { });

    public virtual event Action<ActorMotion> OnActorMotion
    {
        add
        {
            if (!ActorMotionAction.GetInvocationList().Contains(value))
            {
                ActorMotionAction +=value;
            }
        }
        remove
        {
            ActorMotionAction -= value;
        }
    }

    protected int max = int.MaxValue;

    public virtual int Max
    {
        get
        {
            if (max <= 0)
            {
                max = 1;
            }
            return max;
        }

      protected  set
        {
            if (value > 0)
            {
                max = value;
            }
        }
    }

    protected int layer_mask = 0;

    public virtual int Lyaer_mask
    {
        get
        {
            return layer_mask;
        }

       protected  set
        {
            layer_mask = value;
        }
    }
    [SerializeField]
    protected int attack_value=0 ;

    public virtual int Attack_value
    {
        get
        {
            return attack_value;
        }

        protected   set
        {
            if (value >= 0)
            {
                attack_value = 0;
            }
        }
    }

    [SerializeField]
    protected ActorMotion actor_motion;

    public virtual ActorMotion Actor_motion
    {
        get
        {
            if (actor_motion ==null)
            {
                actor_motion = new ActorMotion() ;
            }
            return actor_motion;
        }

        protected set
        {
            actor_motion = value;
            actor_motion.start_speed = value.start_speed > 0f ? value.start_speed : 0f;
        }
    }

    protected int source_attack_value=0;

    public int Source_attack_value { protected set { source_attack_value = value; } get { return source_attack_value; } }

    protected SpawnID spawn_id;

    public virtual SpawnID Spawn_ID { get {
            if (spawn_id == null)
            {
                spawn_id = GetComponent<SpawnID>();
            }
            if (spawn_id.Type==null)
            {
                spawn_id.Type = this;
            }
          

            return spawn_id;
            
        } set { spawn_id = value; } }

    protected void Awake()
    {
        var id = Spawn_ID;
    }

    public abstract void Init(Transform Target, Vector3 StartPos, int layermask, int SourceAttackValue, Vector3? dir =null, Action<ActorMotion> action = null);

    public virtual void InitActorMotion(float startSpeed,Ease ease=Ease.Unset,float  endSpeed = 0f , ActorMotion.UpdateMode updateMode = ActorMotion.UpdateMode.Update)
    {
        Actor_motion.start_speed = startSpeed;
        Actor_motion.ease = ease;
        Actor_motion.end_speed = endSpeed;
        Actor_motion.update_mode = updateMode;
    }

    public virtual void InitTimer( float stepTime = 1f,float overTime = 10f)
    {
        Timer.Overtime = overTime;
        Timer.Step = stepTime;
    }

    public virtual void Clear()
    {
        ClearAction();

        GameManager.Get.Spawn_system.RecoverySpawn(Spawn_ID);

        ClearAction = new Action(() => { });

        ActorMotionAction = new Action<ActorMotion>((ActorMotion am) => { });

        actor_motion.Current = 0f;

        ResetValue();

        gameObject.SetActive(false);
    }

    public virtual void OnStart()
    {
        var id = Spawn_ID; 
    }

    public virtual void ResetValue() 
    {
  
        var source = GameManager.Get.Spawn_system.GetType<Weapon>(Spawn_ID);

        var weapon = this;

        weapon.Actor_motion.start_speed = source.Actor_motion.start_speed;

    }

    protected void OnBecameInvisible()
    {
        if (gameObject.activeSelf)
        {
            Clear();
        }   
    }
}

