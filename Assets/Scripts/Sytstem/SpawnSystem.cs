using System.Collections.Generic;
using UnityEngine;
namespace SpawnManager
{
    [DisallowMultipleComponent]
    public class SpawnSystem :MonoBehaviour
    {
        private readonly Dictionary<int, Queue<GameObject>> m_object_list = new Dictionary<int, Queue<GameObject>>();

        private readonly Dictionary<int, GameObject> m_object_grounp = new Dictionary<int, GameObject>();

        private readonly Dictionary<int, ISpawnID> m_ISpawnID_list = new Dictionary<int, ISpawnID>();

        private GameObject wait_recovery;

        public GameObject WaitRecoveryObj { private set { } get { return wait_recovery; } }

        private SpawnSystem() { }

        private void Awake()
        {
            if (value == null)
            {
                value = this;
            }
            wait_recovery = GetWaitRecoveryObject();
        }

        private static SpawnSystem value;

        public static SpawnSystem Get { get {
                if (value ==null)
                {
                    value = new SpawnSystem();
                }
                return value; }
        }
   

        /// <summary>
        /// 注册生产对象
        /// </summary>
        /// <param name="obj"></param>
        public void RegisterSpawn(SpawnID spawnid)
        {

            spawnid.identification = spawnid.GetHashCode();

            if (!m_object_list.ContainsKey(spawnid.identification))
            {
               
                Queue<GameObject> spawn_queue= new Queue<GameObject>();

                m_object_list.Add(spawnid.identification, spawn_queue);
            }
            if (!m_object_grounp.ContainsKey(spawnid.identification))
            {
                GameObject go = new GameObject(spawnid.identification.ToString());
                go.transform.SetParent(transform);
                go.transform.localPosition = Vector3.down * m_object_grounp.Count;
                m_object_grounp.Add(spawnid.identification, go);
            }

            if (!m_ISpawnID_list.ContainsKey(spawnid.identification))
            {
                m_ISpawnID_list.Add(spawnid.identification, spawnid.Type);
            }
        }
        /// <summary>
        /// 返回产生的游戏对象(活动状态False)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public GameObject OnSpawn(SpawnID spawnid)
        {
            

            if (!m_object_list.ContainsKey(spawnid.identification))
            {
                Debug.LogWarning("Not RegisterSpawn GameObject");
                return null;
            }

            int index = m_object_list[spawnid.identification].Count;
            GameObject newobj;
            if (index> 0)
            {
                newobj = m_object_list[spawnid.identification].Dequeue();
            }
            else
            {
                newobj = Instantiate(spawnid.gameObject);
                newobj.GetComponent<SpawnID>().identification = spawnid.identification;
            }
            newobj.SetActive(false);

            return newobj;
        }

        public void RecoverySpawn(SpawnID spawnid)
        {

            if (!SpawnIdContain(spawnid))
            {
                Debug.LogWarning("Not RegisterSpawn GameObject");
                return;
            }

            int index = m_object_list[spawnid.identification].Count;
            spawnid.gameObject.transform.position = new Vector3(index, 0f, 0f);
            m_object_list[spawnid.identification].Enqueue(spawnid.gameObject);

            if (spawnid.gameObject.activeInHierarchy)
            {
                spawnid.transform.SetParent(m_object_grounp[spawnid.identification].transform);

            }
        }

        public int SpawnListCount(SpawnID spawnid)
        {
            return m_object_list[spawnid.identification].Count;
        }

        public void WaitRecovery(SpawnID spawnid)
        {
            spawnid.transform.SetParent(wait_recovery.transform);
        }

        public bool SpawnIdContain(SpawnID spawnid)
        {
            if (spawnid==null)
            {
                Debug.Log(spawnid);
                return false;
            }
            return m_object_list.ContainsKey(spawnid.identification);
         }

        private GameObject GetWaitRecoveryObject()
        {
            GameObject obj = new GameObject("WaitRecovery");
            obj.transform.SetParent(transform);
            return obj;
        }

        public T GetType<T>(SpawnID spawnid)
        {
            var final = m_ISpawnID_list[spawnid.identification];

            return (T)final;
        }
    }
}
