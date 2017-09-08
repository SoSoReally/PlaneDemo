using System.Collections.Generic;
using UnityEngine;
namespace WeaponManager
{

    public  class WeaponSystem
    {
        bool once = false;

        private WeaponSystem() { m_weapons = new Dictionary<WeaponGet, Weapon>(); }

        private static WeaponSystem value;

        public static WeaponSystem Get
        {
            get {
                if (value ==null)
                {
                    value = new WeaponSystem();
                }

                return value ;}
        }

        private readonly Dictionary<WeaponGet, Weapon> m_weapons;
        
        //public Dictionary<WeaponGet, Weapon> Weapon_dictionary
        //{
        //    get
        //    {
        //        return m_weapons;
        //    }
        //}

        public Weapon GetWeapon(WeaponGet get)
        {
            var ptr= m_weapons[get];
            return ptr; 
        }
        /// <summary>
        /// OnStart OnAwake
        /// </summary>
        public  void LoadAllWeapon()
        {
            if (once)
            {     
                return;
            }
           
            List<Weapon>  weapons =new List<Weapon>(Resources.LoadAll<Weapon>(GamePath.Weapon.Player));   
            foreach (var item in weapons)
            {
                m_weapons.Add(WeaponGet.Enum(EnumActor.Player,item.Enum_weapon) , item);
                GameManager.Get.Spawn_system.RegisterSpawn(item.Spawn_ID);
            }
            weapons = new List<Weapon>(Resources.LoadAll<Weapon>(GamePath.Weapon.Enemy));
            foreach (var item in weapons)
            {
                m_weapons.Add(WeaponGet.Enum(EnumActor.Enemy, item.Enum_weapon), item);
                GameManager.Get.Spawn_system.RegisterSpawn(item.Spawn_ID);
            }
            once = true;
        }
    }
}
public struct WeaponGet
{
    EnumActor enum_actor;

    EnumWeapon enum_weapon;

    private EnumWeapon Enum_weapon
    {
        get
        {
            return enum_weapon;
        }

        set
        {
            enum_weapon = value;
        }
    }

    private EnumActor Enum_actor
    {
        get
        {
            return enum_actor;
        }

        set
        {
            enum_actor = value;
        }
    }

    private WeaponGet(EnumActor Enum_actor,EnumWeapon Enum_weapon    ) { enum_actor = Enum_actor; enum_weapon = Enum_weapon; }

    public static WeaponGet Enum(EnumActor Enum_actor,EnumWeapon Enum_weapon)
    {
        return new WeaponGet(Enum_actor,Enum_weapon) { };
    }
}

public enum EnumActor
{
    Player = 0,
    Enemy = 1,
}
public enum EnumWeapon
{
    Default = 0,
    Cone,
    NotDestory
}