using System;
using UnityEngine;

public class Cone : Weapon
{
    public Vector3 target;

    public override EnumWeapon Enum_weapon
    {
        get
        {
            return EnumWeapon.Cone;
        }
    }
    private void OnEnable()
    {
        Timer.Start = Time.time;

    }
    public void Start()
    {

        base.OnStart();

        actor_motion.transform = transform;

        Timer.OnOverTime += Clear;
        
    }

    public void Update()
    {
        ActorMotionAction(actor_motion);
    }
   
    public override void Clear()
    {
        /*
        
        */
        base.Clear();
    }

    float speed  = 20f;

    bool over = false;

    public void UpdateMove()
    {
        
        Vector3 pos = transform.position;

        float anlge = Vector3.Angle(transform.up,target-pos);

        float distance = Vector3.Distance(pos, target);

        if (distance < 0.5f)
        {
            over = true;
        }

        if (pos == Vector3.zero)
        {
            pos = Vector2.one * 0.01f;
        }

        if (target == Vector3.zero)
        {
            target = Vector2.one * 0.01f;
        }

        Vector3 cross = Vector3.Cross(pos, (target - pos).normalized);

        float rotationDir = Mathf.Abs(cross.z) / cross.z;

        if (cross==Vector3.zero)
        {
            rotationDir = 1f;
        }

        float frap_rotation = anlge / (distance / speed);


        frap_rotation *= (1f +(anlge/Time.deltaTime/ 180f)*0.1f);


        if (!over)
        {
            transform.Rotate(Vector3.forward, frap_rotation * Time.deltaTime * rotationDir);
           
        }

        transform.position += transform.up * Time.deltaTime * speed;
    }

    public override void Init(Transform Target, Vector3 StartPos, int layermask, int SourceAttackValue, Vector3? dir=null, Action<ActorMotion> action = null)
    {

        transform.position = StartPos;

        gameObject.SetActive(true);

        actor_motion.TargetPos = Target.position;

        Source_attack_value = SourceAttackValue;

        if (dir!=null)
        {
            transform.localEulerAngles = (Vector3)dir;
        }

        gameObject.layer = Lyaer_mask = layermask;

        actor_motion.IsOver = false;

        if (action != null)
        {
            OnActorMotion += action;
        }
        // Actor_motion.dir = dir;
    }


}
