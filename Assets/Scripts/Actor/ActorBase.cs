using System;
using System.Linq;
using UnityEngine;

public abstract class ActorBase : MonoBehaviour, ILifeValue, IAttack
{

    [SerializeField]
    protected int life_value;

    public virtual int Life_value { get { return life_value > 0 ? life_value : 0; } set { life_value = value; } }

    [SerializeField]
    protected int attack_value;

    public virtual int Attack_value { get { return attack_value > 0 ? attack_value : 0; } set { attack_value = value; } }

    [SerializeField]
    protected ActorEnums actor_Enums;

    public abstract ActorEnums Actor_Enums
    {  get; }

    public virtual EffectEnum DealthParticle { get {  return actor_Enums.dealtheffect; } }

    public virtual EnumAudio DealthAudio { get { return actor_Enums.dealthaudio;  } }

    public virtual EnumWeapon EnumWeapon
    {
        get { return actor_Enums.weapon; }
    }

    public virtual EnumActor EnumActor { get { return actor_Enums.actor; } }

    [SerializeField]
    protected Weapon m_weapon;

    public Weapon UsingWeapon
    {
        get
        {
            return m_weapon;
        }
            
    }



    protected Action<int> HurtAction=new Action<int>((int i) => {});

    public virtual event Action<int> OnHurt { add {
            if (!HurtAction.GetInvocationList().Contains(value))
            {
                HurtAction += value;
            }
        }
            remove
        {
            HurtAction -= value;
        }
    }

    protected Action AttackAction = new Action(() => {});

    public virtual event Action OnAttack { add {
            if (!AttackAction.GetInvocationList().Contains(value))
            {
                AttackAction += value;
            }

        } remove { AttackAction -= value; }  }

    protected abstract void Hurt(int value);

    protected abstract void Attack();

    protected abstract void Move();

    public virtual void Dealth()
    {
        GameManager.Get.Sound_system.Current.PlayOneShot(AudioGet.Enum(DealthAudio ));
        GameManager.Get.Particle_system.PlayOnPos(DealthParticle, transform.position);
    }

    [SerializeField]
    private Timer timer;

    public Timer Timer
    {
        get
        {
            if (timer == null)
            {
                timer = new Timer();
            }
            return timer;
        }
    }
}
[Serializable]
public struct ActorEnums
{
    public  EffectEnum dealtheffect;
    public  EnumAudio dealthaudio;
    public  EnumWeapon weapon;
    public  EnumActor actor;
}