using UnityEngine;

public class Tag {
    public static readonly string  Bullet = "Bullet";
    public static readonly string Player = "Player";

}


public static class LayerCustom {
    enum LayerCustomEnum
    {
        Enemy = 1,
        Player = 2,
        EnemyBullet = 3,
        PlayerBullet = 4,
        EnmeyBulletCanDestory = 5
    }
    public static readonly int Enemy = LayerMask.NameToLayer( SortingLayer.layers[(int)LayerCustomEnum.Enemy].name );
    public static readonly int Player = LayerMask.NameToLayer(SortingLayer.layers[(int)LayerCustomEnum.Player].name);
    public static readonly int EnemyBullet = LayerMask.NameToLayer(SortingLayer.layers[(int)LayerCustomEnum.EnemyBullet].name);
    public static readonly int PlayerBullet = LayerMask.NameToLayer(SortingLayer.layers[(int)LayerCustomEnum.PlayerBullet].name);
    public static readonly int EnemyBulletCanDestory = LayerMask.NameToLayer(SortingLayer.layers[(int)LayerCustomEnum.EnmeyBulletCanDestory].name);

}
