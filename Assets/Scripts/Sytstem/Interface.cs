using System;
public interface ILifeValue
{
    int Life_value { set; get; }

    event Action<int> OnHurt;

    void Dealth();

    EffectEnum DealthParticle { get; }

    EnumAudio DealthAudio { get; }
}

public interface IAttack
{

    event Action OnAttack;

    int Attack_value { set; get; }

    EnumWeapon EnumWeapon { get; }

    Weapon UsingWeapon { get; }
}