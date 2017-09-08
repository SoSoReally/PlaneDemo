using System;
using UnityEngine;

public class Bullet : Weapon {


    public override EnumWeapon Enum_weapon
    {
        get
        {
          return  EnumWeapon.Default;
        }
    }
    protected void Start()
    {
        base.OnStart();

        actor_motion.transform = transform;

       // actor_motion.update_mode = ActorMotion.UpdateMode.FixUpdate;
    }

    protected void OnEnable()
    {
        Timer.Start = Time.time;
   
    }
    protected void FixedUpdate()
    {
     
         ActorMotionAction(Actor_motion);
    }
    protected void Update()
    {
        Timer.Update(Timer);
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    var other = collision.gameObject;
    //    if (other.tag != Tag.Player && other.GetComponent<SpawnID>() == null)
    //    {

    //        Debug.Log(other.name);
    //        Clear();
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
            Clear();
    }
    public override void Clear()
    {
        ///

       

        ///
        base.Clear();
    }
    public override void Init(Transform Parent, Vector3 StartPos, int layermask, int SourceAttackValue, Vector3? dir = null, Action<ActorMotion> action = null)
    {

        transform.position = StartPos;
        gameObject.SetActive(true);
        transform.SetParent(Parent);
        Source_attack_value = SourceAttackValue;
        gameObject.layer = Lyaer_mask = layermask;
       if (action != null)
        {
                OnActorMotion += action;
        }
        Actor_motion.dir = (Vector3)dir;
    }


}
