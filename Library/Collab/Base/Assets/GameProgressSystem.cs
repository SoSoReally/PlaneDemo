using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
namespace GameProgressManager
{
    [RequireComponent(typeof(Animation)), DisallowMultipleComponent]
    public class GameProgressSystem : MonoBehaviour
    {

        private int index=0;

        private float time = 0f;

        private GameProgressSystem() { }
        
        private static GameProgressSystem value;

        public static GameProgressSystem Get { get { return value; } }

        [SerializeField]
        private List<TimeEvent> list_time_evet=new List<TimeEvent>();
        private List<TimeEvent> List_time_evet { get { return list_time_evet; } }

        private void Awake()
        {
            if (value == null)
            {
                value = this;

            }
        }

        private void Update()
        {
            ProgressUpdate();
        }

        private void ProgressUpdate()
        {
            time += Time.deltaTime;
            if (List_time_evet.Count < index+1)
            {
                return;
            }
            if (List_time_evet[index].start < time)
            {
                Debug.Log("??");
                list_time_evet[index].action?.Invoke();
                index++;
            }
            Debug.Log(time);
        }

        public void SetGameProgress(int index)
        {

        }
        // Use this for initialization
        public void CreatEnemyOne()
        {
            var enemy = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Capsule);

            GameManager.Get.Spawn_system.RegisterSpawn(enemy.Spawn_ID);

            GameObject[] objs = new GameObject[4];
            for (int i = 0; i < 4; i++)
            {
                objs[i] = GameManager.Get.Spawn_system.OnSpawn(enemy.Spawn_ID);
                objs[i].gameObject.SetActive(true);
                objs[i].transform.position = new Vector3(i, i, 0f);
            }

        }
        public void CreatEnemySecond()
        {

        }
        public void CreatEnemyThird()
        {

        }
    }
}
[Serializable]
public class TimeEvent
{

    public float start;

    public UnityEvent action;

}