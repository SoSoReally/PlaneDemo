using DG.Tweening;
using System;
using UnityEngine;
[Serializable]
public class ActorMotion:ITrack
{
    [HideInInspector]
    public Transform transform;

    public float start_speed = 1f;

    public UpdateMode update_mode = UpdateMode.Update;

    [HideInInspector]
    public Vector3 dir = Vector3.zero;

    public float end_speed = 1f;

    public float duration = 1f;

    private float current = 0f;

    public Ease ease=Ease.Unset;

    public float Current
    {
        get
        {
            return Mathf.Clamp01(current);
        }
        set
        {
            current =  value;
        }
    }

    public ActorMotion()
    {

    }

    public interface IMotion
    {
        event Action<ActorMotion> OnActorMotion;
        ActorMotion Actor_motion {   get; }
    }

    public enum UpdateMode
    {
        FixUpdate,
        Update,
        Latedate
    }


    public Vector3 TargetPos
    {
        set; get;
    }

    public Transform TargetTransform { set; get; }

    public bool IsOver { set; get; }

    public ActorMotion Clone()
    {
        return (ActorMotion)MemberwiseClone();
    }


}
interface ITrack
{

    Vector3 TargetPos { set; get; }

    Transform TargetTransform { set; get; }

    bool IsOver { set; get; }
}