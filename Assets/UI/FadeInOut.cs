using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;
 
public class FadeInOut : MonoBehaviour
{
    public event Action OnFadeComplete;  //Camera Fade In,Out 완료시 호출
 
    [SerializeField] private bool fadeInOnStart = true;
    [SerializeField] private bool fadeInOnSceneLoad = false;
 
    [SerializeField] private float fadeDuration = 2f;
 
    [SerializeField] private Image fadeImage;
    [SerializeField] private Color fadeColor = Color.black;
 
    private bool isFading;
    private float fadeStartTime;
    private Color fadeOutColor;
 
    public bool IsFading
    {
        get
        {
            return isFading;
        }
    }
 
    private void Awake()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;

        fadeOutColor = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);
        fadeImage.enabled = true;
    }
 
 
    // Start is called before the first frame update
    void Start()
    {
        if(fadeInOnStart)
        {
            fadeImage.color = fadeColor;
            FadeIn();    
        }
    }
 
    private void HandleSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(fadeInOnSceneLoad)
        {
            fadeImage.color = fadeColor;
            FadeIn();
        }
    }
 
 
    public void FadeIn()
    {
        if (isFading)
            return;
 
        StartCoroutine(BeginFade(fadeColor, fadeOutColor, fadeDuration));
    }
 
    public void FadeOut()
    {
        if (isFading)
            return;
 
        StartCoroutine(BeginFade(fadeOutColor,fadeColor, fadeDuration));
    }
 
    public IEnumerator BeginFadeOut()
    {
        yield return StartCoroutine(BeginFade(fadeOutColor, fadeColor, fadeDuration));
    }
 
    public IEnumerator BeginFadeIn()
    {
        yield return StartCoroutine(BeginFade(fadeColor, fadeOutColor, fadeDuration));
    }
 
    private IEnumerator BeginFade(Color startCol, Color endCol , float duration)
    {
        //Start Fade
        isFading = true;
 
        float timer = 0f;
        while(timer <= duration)
        {
            fadeImage.color = Color.Lerp(startCol, endCol, timer / duration);
 
            timer += Time.deltaTime;
 
            yield return null;
        }
 
        isFading = false;
 
        if (OnFadeComplete != null)
            OnFadeComplete();
    }
 
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }
}