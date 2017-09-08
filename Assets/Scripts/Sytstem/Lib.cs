using DG.Tweening;
using UnityEngine;

public static class ActorMotionMode
{
    static float StepTime(ActorMotion.UpdateMode update_mode)
        {
            float step_time = 0f;
            switch (update_mode)
            {
                case ActorMotion.UpdateMode.FixUpdate:
                    step_time = Time.fixedDeltaTime;
                    break;
                case ActorMotion.UpdateMode.Update:
                    step_time = Time.deltaTime;
                    break;
                case ActorMotion.UpdateMode.Latedate:
                    step_time = Time.smoothDeltaTime;
                    break;
                default:
                    break;
            }
            return step_time;
        }

    static float StepSpeed(ActorMotion actor_motion)
    {
        float step_time = StepTime(actor_motion.update_mode);
        var am = actor_motion;
        float speed = 0f;
        if (am.ease == Ease.Unset)
        {
            speed = am.start_speed;
        }
        else
        {
            speed = DOVirtual.EasedValue(am.start_speed, am.end_speed, am.Current, am.ease);
            am.Current += step_time * (1f / am.duration);
        }
        return speed ;
    }

    public static void Line(ActorMotion actor_motion)
    {

        float step_time = StepTime(actor_motion.update_mode);

        float speed = StepSpeed(actor_motion);
                
        var target = actor_motion.transform;
            
        target.Translate(speed * actor_motion.dir* step_time, Space.World);
    }

    public static void Tracker (ActorMotion actor_motion)
    {
        float step_time = StepTime(actor_motion.update_mode);

        ActorMotion am = actor_motion;

        Transform transform = am.transform;

        Vector3 target = am.TargetPos;

        Vector3 pos = transform.position;

        float speed = StepSpeed(am);

        float anlge = Vector3.Angle(transform.up,target-pos);

        float distance = Vector3.Distance(pos, target);

        if (distance < 0.5f)
        {
            am.IsOver = true;
        }

        if (pos == Vector3.zero)
        {
            pos = Vector2.one * 0.01f;
        }

        if (target == Vector3.zero)
        {
            target = Vector2.one * 0.01f;
        }

        Vector3 cross = Vector3.Cross(transform.up, (target - pos).normalized);

        float rotationDir = Mathf.Abs(cross.z) / cross.z;

        if (cross==Vector3.zero)
        {
            if (Vector3.Dot(pos,target)>0)
            {
                rotationDir = 1f;
            }else
            {
                rotationDir = -1f;
            }
           
        }

        float frap_rotation = anlge / (distance / speed);


        frap_rotation *= (1f +(anlge/ step_time / 180f)*0.1f);


        if (!am.IsOver)
        {
            transform.Rotate(Vector3.forward, frap_rotation * step_time * rotationDir);
           
        }

        transform.position += transform.up * step_time * speed;
    }

    public static void FloatMove(ActorMotion actor_mation)
    {
       // ActorMotion am = actor_mation;


    }

    public static void CapulseMove(ActorMotion actor_mation)
    {
        ActorMotion am = actor_mation;

        if (Vector3.Distance( am.transform.position,am.TargetPos)<0.5f)
        {
            am.TargetPos = -am.TargetPos;

            var pos = am.transform.position;

            pos.x = -pos.x;

            am.transform.position = pos;

            am.dir = -am.dir;
        }

        Line(am);
    }

    public static void RotationAround(ActorMotion actor_mation)
    {
        float speed = StepSpeed(actor_mation);

        ActorMotion ac = actor_mation;

        ac.transform.RotateAround(ac.TargetPos, Vector3.forward,  speed);

    }
}


