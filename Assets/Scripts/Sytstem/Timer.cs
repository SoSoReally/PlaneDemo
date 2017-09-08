using System;

using UnityEngine;
[Serializable]
public class Timer
{
    [SerializeField]
    protected float step;
    public float Step
    {
        get
        {
            return step;
        }
        set
        {
            step = value;
        }
    }
    [SerializeField]
    protected float overtime;
    public float Overtime
    {
        get
        {
            return overtime;
        }

        set
        {
            overtime = value;
        }
    }

    protected float start;
    public float Start { set { is_overtime = false; start = value; } protected get { return start; } }

    protected float Real_over_time { get { return Start + Overtime; } }

    protected float step_start = 0f;

    public Timer() { }
    public Timer(float step)
    {
        Step = step;
        Start = Time.time;
    }


    public event Action OnStep = new Action(() => { });

    public event Action OnOverTime = new Action(() => { });

    public static bool TimerDealyEnd(Timer timer, bool instantly = true)
    {
        if (!instantly)
        {
            timer.step_start = Time.time + timer.Step;
        }
        if (Time.time > timer.step_start)
        {
            timer.step_start = Time.time + timer.Step;
            return true;
        }
        else
        {
            return false;
        }
    }

    protected bool is_overtime = false;
    public bool Is_overtime { protected set { } get { return is_overtime; } }



    protected virtual void Update()
    {
        if (Time.time > step_start)
        {
            step_start = Time.time + Step;
            OnStep.Invoke();
        }

        if (Real_over_time < Time.time && !is_overtime)
        {

            OnOverTime.Invoke();
            is_overtime = true;
        }


    }
    public static void Update(Timer timer)
    {
        timer.Update();
    }


}
public class Timer<T> : Timer
{
    public T temp;

    public Timer()
    {
    }

    public Timer(float step) : base(step)
    {
    }

    public new event Action<T> OnStep = new Action<T>((T) => { });

    public new event Action<T> OnOverTime = new Action<T>((T) => { });

    protected override void Update()
    {

        if (Time.time > step_start)
        {
            step_start = Time.time + Step;
            OnStep.Invoke(temp);
        }

        if (Real_over_time < Time.time)
        {
            OnOverTime.Invoke(temp);
        }
    }

}