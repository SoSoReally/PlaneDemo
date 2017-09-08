using System.Collections.Generic;
using UnityEngine;
namespace CreatEnemyManager
{
    [DisallowMultipleComponent]
    public class CreatEnemySystem : MonoBehaviour
    {
        bool once = false;


        private CreatEnemySystem() { }

        private void Awake()
        {
            if (value == null)
            {
                value = this;
            }
        }

        private static CreatEnemySystem value;

        public static CreatEnemySystem Get { get { return value; } }

        private Dictionary<EnemyStyle, Enemy> Enemy_dictionary = new Dictionary<EnemyStyle, Enemy>();
        //OnStart or Awake  on GameManager 
        public void LoadAllEnemy()
        {
            if (once)
            {
                return;
            }
            var ptr = Resources.LoadAll<Enemy>(GamePath.AllEnemy);

            foreach (var item in ptr)
            {
                Enemy_dictionary.Add(item.Enemy_style, item);
            }
            once = true;
        }

        public Enemy GetEnemy(EnemyStyle style)
        {
            var enemy = Enemy_dictionary[style];

            GameManager.Get.Spawn_system.RegisterSpawn(enemy.Spawn_ID);

            return enemy;
        }
    }
}
public enum EnemyStyle
{
    Capsule,
    Cube,
    Sphere,
    Cylinder,
    Text
}
