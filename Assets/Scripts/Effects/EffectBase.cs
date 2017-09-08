using UnityEngine;
[RequireComponent(typeof(ParticleSystem),typeof(SpawnID))]
public abstract class EffectBase : MonoBehaviour ,ISpawnID{

    protected new ParticleSystem particleSystem;

    public ParticleSystem Particle { set { } get {
            if (!particleSystem)
            {
                particleSystem = GetComponent<ParticleSystem>();
                var ma = particleSystem.main;
                ma.playOnAwake = false;
            }
            return particleSystem;
        } }
    protected void Awake()
    {
        var id = Spawn_ID;
    }

    public abstract EffectEnum EffectEnum { get; }

    protected SpawnID spawn_id;

    public SpawnID Spawn_ID
    {
        get
        {
            if (spawn_id==null)
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
         set { }
    }

    public virtual void Play()
    {
        if( Particle.isPlaying)
        {
            return;
        }
        particleSystem.Play();
    }

    internal void Play(bool withChildren)
    {
        if (Particle.isPlaying)
        {
            return;
        }
        particleSystem.Play(withChildren);
    }

    public Timer timer;

    public virtual void Clear()
    {
        GameManager.Get.Spawn_system.RecoverySpawn(Spawn_ID);

        gameObject.SetActive(false);
    }

    protected  virtual void OnStart()
    {
        timer.Overtime = Particle.main.duration;

        timer.OnOverTime += Clear;

        var id = Spawn_ID;
    }

    protected virtual void OnEnable()
    {
        
        timer.Start = Time.time;
    }


    public virtual void ResetValue()
    {
        
    }
}

public enum EffectEnum
{
    Dealth,
    FallingDealith
}
