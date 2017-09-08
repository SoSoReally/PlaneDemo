public class DealthEffect :EffectBase {

    private void Start()
    {
        base.OnStart();
        var main = Particle.main;
        main.loop = false;
    }

    public override EffectEnum EffectEnum
    {
        get
        {
            return EffectEnum.Dealth;
        }
    }

    void Update()
    {
        Timer.Update(timer);
    }
}
