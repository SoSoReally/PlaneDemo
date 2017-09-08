using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
public class Fade : MonoBehaviour {
    public Material mat;

    public bool isFade=false;

    public Action FadeInOnEnd = new Action(() => { });

    public Action FadeOutOnEnd = new Action(() => { });

    private int nameId = Shader.PropertyToID("_SliderColor");


    // Use this for initialization
    private void Start()
    {
        mat.SetColor("_SliderColor", Color.white);
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
            Graphics.Blit(source, destination, mat);    
    }
    public void FadePlay()
    {

       // tween = mat.DOColor(Color.black, "_SliderColor", 0.5f).OnComplete(OnComplete).ChangeStartValue(Color.white);

        //isFade = true;
    }
    public void FadePlayPingPang(TweenCallback OnStart)
    {
       // tween = null;
        
       // mat.DOColor(Color.white, "_SliderColor", 0.5f).ChangeStartValue(Color.black);

    }

    public IEnumerator  FadeIn()
    {
        bool isfade = true;

        float time = 0f;

        float duration = 0.5f;

        float speed = 1 / duration;

        mat.SetColor(nameId, Color.white);

        while (isfade)
        {
            time += Time.deltaTime;
            if (time>duration)
            {
                isfade = false;
            }

            mat.SetColor(nameId, Color.Lerp(Color.white,Color.black, time*speed));

            yield return new WaitForEndOfFrame();
        }

        FadeInOnEnd();

        FadeInOnEnd = new Action(() => { });
    }

    public IEnumerator FadeOut()
    {
        bool isfade = true;

        float time = 0f;

        float duration = 0.5f;

        float speed = 1 / duration;

        mat.SetColor(nameId, Color.black);

        while (isfade)
        {
            time += Time.deltaTime;
            if (time > duration)
            {
                isfade = false;
            }


            mat.SetColor(nameId, Color.Lerp(Color.black, Color.white, time * speed));

            yield return new WaitForEndOfFrame();
        }

        FadeOutOnEnd();

        FadeOutOnEnd = new Action(() => { });
    }

}
