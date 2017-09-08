using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using SpawnManager;
using WeaponManager;
using SoundManager;
using UnityEngine.SceneManagement;
using CreatEnemyManager;
using ParticleManager;
using InputManager;
using GameProgressManager;
using DG.Tweening;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour {
    private GameManager() { }

    private static GameManager gm;

    public static GameManager Get { get { return gm; } }

    public SpawnSystem Spawn_system { get { return SpawnSystem.Get;} }

    public WeaponSystem Weapon_system { get { return WeaponSystem.Get; } }

    public SoundSystem Sound_system { get { return SoundSystem.Get; } }

    public CreatEnemySystem CreatEnemy_system { get { return CreatEnemySystem.Get; } }

    public GameParticleSystem Particle_system { get { return GameParticleSystem.Get; } }

    public GameProgressSystem GameProgress_system { get { return GameProgressSystem.Get; } }

    public GameObject UI;

    public InputSystem Input_system { get { return InputSystem.Get; } }

    private  List<Timer> DealyRun  = new List<Timer>();

    private  List<Timer<int>> DealyRunInt = new List<Timer<int>>();

    public event Action Game_over = new Action(()=> { });
    
    [HideInInspector]
    public GameObject Player;

    public GameObject ResourcesPlayer { private set; get; }

    private Camera mainCamera;

    public Camera MainCamera { get { return mainCamera = mainCamera ?? Camera.main; } }

    private void Awake()
    {
        if (gm==null)
        {
            gm = this;
        }
        DontDestroyOnLoad(gameObject.transform.root);

        Weapon_system.LoadAllWeapon();

        Sound_system.LoadAllAudioClip();

        CreatEnemy_system.LoadAllEnemy();

        Particle_system.LoadAllParticle();

        LoadPlayer();
    }

    private void Start()
    {
        SceneManager.LoadScene(1);
    }

    private void Update()
    {
        for (int i = 0; i < DealyRun.Count; i++)
        {
            Timer.Update(DealyRun[i]);
        }
        for (int i = 0; i < DealyRunInt.Count; i++)
        {
            Timer.Update(DealyRunInt[i]);
         
        }
    }

    public Snap CameraSnap(Camera main)
    {
        Snap snap;

        snap.width = main.pixelWidth;

        snap.height = main.pixelHeight;

        snap.rect = main.rect;

        snap.pixelRect = main.pixelRect;

        return snap;
    }

    public void DealyInvoke(float delay, Action action)
    {
        Timer timer = new Timer() {Overtime = delay };
        DealyRun.Add(timer);
        timer.OnOverTime += action;
        timer.OnOverTime += () => { DealyRun.Remove(timer); };
    }

    public void DealyInvoke(int index,float delay, Action<int> action )
    {
        Timer<int> timer = new Timer<int>() { Overtime = delay};
        DealyRunInt.Add(timer);
        timer.temp = index;
        timer.Start = Time.time;
        timer.OnOverTime += action;
        timer.OnOverTime += (T) => { DealyRunInt.Remove(timer); };
    }

    public void GameOver()
    {

        GameReset();

    }

    /// <summary>
    ///            1
    ///            |
    ///            |
    ///    -1----- 0 -----1
    ///            |
    ///            |
    ///           -1
    /// </summary>
    /// <param name="vec2"></param>
    /// <returns></returns>
    public Vector3 ScreenUV(Vector2 vec2)
    {
        var snap = CameraSnap(MainCamera);

        vec2 /= 2f;

        vec2 += Vector2.one * 0.5f;

        float x = vec2.x * snap.width;

        float y = vec2.y * snap.height;

        Vector3 final = new  Vector3(x, y,-MainCamera.transform.position.z);

        Vector3 pos = MainCamera.ScreenToWorldPoint(final);

        return pos;
    }

    private void GameReset()
    {
      

        var fade = MainCamera.GetComponent<Fade>();

        StartCoroutine(fade.FadeIn());

        fade.FadeInOnEnd += () =>
        {
            StartCoroutine(fade.FadeOut());

            if (Game_over != null)
            {

                Game_over.Invoke();
            }

            Game_over = new Action(() => { });

            GameProgress_system.ResetPlay();

            DealyRun = new List<Timer>();

            DealyRunInt = new List<Timer<int>>();

            var weapons = FindObjectsOfType<SpawnID>();

            foreach (var item in weapons)
            {
                item.Type.Clear();
            }
        };  
    }

    private void LoadPlayer()
    {
        ResourcesPlayer = Resources.Load<GameObject>(GamePath.Player);
    }
}

public struct Snap
{
    public float width;

    public float height;

    public Rect pixelRect;

    public Rect rect;

}












