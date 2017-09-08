using UnityEngine;

public class BulletSecond : Bullet {

    public override EnumWeapon Enum_weapon
    {
        get
        {
            return EnumWeapon.NotDestory;
        }
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerCustom.PlayerBullet)
        {
            other.GetComponent<Weapon>().Clear();
        }
    }

}
