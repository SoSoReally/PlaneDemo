using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;


namespace GameProgressManager
{

    [Serializable]
    public class TimeEvent:IComparable<TimeEvent>
    {

        public float start;

        public UnityEvent action;
        public TimeEvent() { action = new UnityEvent(); }


        public int CompareTo(TimeEvent other)
        {
            if (start>other.start)
            {
                return 1;
            }
            else if (start == other.start)
            {
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }

    [RequireComponent(typeof(Animation)), DisallowMultipleComponent]
    public class GameProgressSystem : MonoBehaviour
    {

        private int index=0;

        public int Index { get { return index; } }

        private float time = 0f;

        private GameProgressSystem() { }
        
        private static GameProgressSystem value;

        public static GameProgressSystem Get { get { return value; } }

        private int m_active_enemy = 0;

        private bool m_progressStop = false;

        public bool ProgressStop { get { return m_progressStop; } }

        public bool ProgressOver { set; get; }

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
        private void Start()
        {
             var palyer = Instantiate(GameManager.Get.ResourcesPlayer, GetFinalPos(new Vector2(0f, -0.2f)), Quaternion.identity);

            GameManager.Get.Player = palyer.transform.GetChild(0).gameObject;

            pc = GameManager.Get.Player.GetComponent<PlayerControl>();

            ProgressOver = false;

            DOTween.Clear();
        }


        private void Update()
        {
            ProgressUpdate();
        }

        PlayerControl pc;

        public void OnGUI()
        {
            GUILayout.TextArea(pc.Life_value.ToString());
        }

        public void ResetPlay()
        {
            m_active_enemy = 0;
        }

        public void Sort()
        {
            List_time_evet.Sort();
        }

        private void ProgressUpdate()
        {

            if (m_active_enemy>0)
            {
                m_progressStop = true;
                Mathf.Floor(time);
                return;
            }
            m_progressStop = false;

            time += Time.deltaTime;

            if (List_time_evet.Count < index+1)
            {
                return;
            }
            if (List_time_evet[index].start < time)
            {
                list_time_evet[index].action.Invoke();
                index++;
            }  
        }

        public void SetGameProgress(int index)
        {
            this. index = index;
            time -= 1f;
        }
        
        private Vector3 GetFinalPos(Vector2 vec2)
        {
            return  GameManager.Get.ScreenUV(vec2);
        }

        public void CreatEnemyOne()
        {
            var enemy = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Capsule);

            GameObject[] objs = new GameObject[5];

            m_active_enemy = 5;

            for (int i = 0; i < 5; i++)
            {
                objs[i] = GameManager.Get.Spawn_system.OnSpawn(enemy.Spawn_ID);
                
                GameManager.Get.DealyInvoke(i,  i * 0.75f, (index) =>
                {
                    objs[index].SetActive(true);

                    int angle = index + 1;

                    objs[index].transform.position= Ex.Vec3GetDirForAngle(angle*30f) * 10f;

                    objs[index].transform.DOMove(Ex.Vec3GetDirForAngle(angle*30f) * 5f, 2f).SetEase(Ease.InOutQuad);

                    objs[index].GetComponent<Enemy>().OnClear += () => { m_active_enemy--; };
                });
            }
 

        }

        public void CreatEnemySecond()
        {
            var enemy_source = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Sphere);

            GameObject[] enemys = new GameObject[5];

            m_active_enemy = 5;

            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i] = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);


                GameManager.Get.DealyInvoke(i,i+1, (index) =>{

                    var enemy= enemys[index].GetComponent<Enemy>();

                    enemy.gameObject.SetActive(true);

                    var pos = GameManager.Get.ScreenUV(new Vector2(0.5f, 1.1f));

                    enemy.Init(pos, Vector3.down, ActorMotionMode.CapulseMove);

                    enemy.InitTimer(0.7f);

                    enemy.InitActorMotion(5f);

                    pos.y = -pos.y;

                    enemy.Actor_motion.TargetPos = pos;

                    enemy.OnClear += () => { m_active_enemy--; };

                });
            }
        }

        public void CreatEnemyThird()
        {
            GameObject[] enemys = new GameObject[5];

            m_active_enemy = 5;

            var enemys_cube = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Cube);

            var enemys_capsule = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Capsule);

            enemys[0] = GameManager.Get.Spawn_system.OnSpawn(enemys_cube.Spawn_ID);

            DOVirtual.DelayedCall(5f*1.3f, ()=> { enemys[0].SetActive(true); }) ;

            GameObject go = new GameObject();

            go.transform.position = Vector3.zero;        

            var cube = enemys[0].GetComponent<Enemy>();

            cube.Init(Vector3.zero);

            cube.OnClear += () => { m_active_enemy--;if (m_active_enemy == 0) { Destroy(go); } };

            for (int i = 1; i < enemys.Length; i++)
            {
                enemys[i] = GameManager.Get.Spawn_system.OnSpawn(enemys_capsule.Spawn_ID);

                enemys[i].transform.SetParent(go.transform);


                GameManager.Get.DealyInvoke(i, i*1.3f, (index) =>
                {
                    var enemy= enemys[index].GetComponent<Enemy>();

                    enemy.gameObject.SetActive(true);

                    enemy.OnClear += () => { m_active_enemy--; if (m_active_enemy == 0) { Destroy(go); } };

                });

            }

            float distance = 1.5f;

            enemys[1].transform.position = go.transform.up*distance;

            enemys[2].transform.position = go.transform.right * distance;

            enemys[3].transform.position = -go.transform.up * distance;

            enemys[4].transform.position = -go.transform.right * distance;

            go.transform.DORotate(new Vector3(0f, 0f, 180f), 1.5f, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Incremental).SetEase(Ease.Flash);
        }

        public void StepFour()
        {
            Vector3[] paths = new Vector3[6];
            paths[0] = GetFinalPos(new Vector2(-1.1f, 0.5f));
            paths[1] = GetFinalPos(new Vector2(-0.6f, 0f));
            paths[2] = GetFinalPos(new Vector2(0f, -0.5f));
            paths[3] = GetFinalPos(new Vector2(0.2f,0f));
            paths[4] = GetFinalPos(new Vector2(0f, 0.5f));
            paths[5] = GetFinalPos(new Vector2(-0.5f, 0.3f));


            var enemy_source = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Sphere);

            GameObject[] enemys = new GameObject[5];

            m_active_enemy = 5;

            enemys[0] = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);

            enemys[0].transform.position = paths[0];

            var tween = enemys[0].transform.DOPath(paths, 2f, PathType.CatmullRom, PathMode.TopDown2D);

            tween.ForceInit();

            tween.SmoothRewind();

            var finalpath = tween.PathGetDrawPoints(10);

            tween.Kill();

            for (int i = 0; i < enemys.Length; i++)
            {
                enemys[i] = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);

                GameManager.Get.DealyInvoke(i, i + 1, (index) => {

                    var enemy = enemys[index].GetComponent<Enemy>();

                    var pos = finalpath[0];

                    enemy.gameObject.SetActive(true);

                    int length = (int)((float)finalpath.Length * (1f - index * 0.1f));

                    Vector3[] everypath = new Vector3[length];

                    for (int j = 0; j < everypath.Length; j++)
                    {
                        everypath[j] = finalpath[j];
                    }

                    enemy.Init(pos);

                    enemy.InitTimer(0.7f);

                    enemy.InitActorMotion(5f);

                    enemy.transform.DOPath(everypath, 3f, PathType.Linear, PathMode.TopDown2D, 1);

                    enemy.OnClear += () => { m_active_enemy--; };

                });
            }
        }

        public void StepFive()
        {

            var enemy_source = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Cube);

            m_active_enemy = 3;

            Vector3[] startpos = new Vector3[3]
            {
                GetFinalPos(new Vector2(0.5f,0.5f)),
                GetFinalPos(new Vector2(0f,0f)),
                GetFinalPos(new Vector2(-0.5f,0.5f)),
            };

            for (int i = 0; i < 3; i++)
            {
                var enemy = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);

                enemy.SetActive(true);

                enemy.transform.position = startpos[i];

                enemy.GetComponent<Enemy>().OnClear += () => { m_active_enemy--; };
            }
        }

        public void CreatEnemyText()
        {
            var enemy_source = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Cube);

            var enemy = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);

            enemy.SetActive(true);

            m_active_enemy++;

            enemy.transform.position = GameManager.Get.ScreenUV(new Vector2(0.5f,0f));

            enemy.GetComponent<Enemy>().OnClear += () => { m_active_enemy--; };
        }

        public void StepSix()
        {
            var enemy_source = GameManager.Get.CreatEnemy_system.GetEnemy(EnemyStyle.Text);

            var enemy = GameManager.Get.Spawn_system.OnSpawn(enemy_source.Spawn_ID);

            enemy.SetActive(true);

            enemy.transform.position = GameManager.Get.ScreenUV(new Vector2( 0f, 0.5f));

            m_active_enemy++;

            enemy.GetComponent<Enemy>().OnClear += () => { m_active_enemy--; };

            Vector3 left = GameManager.Get.ScreenUV(new Vector2(-0.8f, 0.5f));



            enemy.transform.DOMoveX(left.x, 1f).SetEase(Ease.Flash).OnComplete(() =>
            {
                enemy.transform.DOMoveX(-left.x, 2f).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.Flash);


            });
        }
    }
}
