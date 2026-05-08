using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UILoadingScreen: Singleton<UILoadingScreen>
{
    [SerializeField]
    private GameObject LoadingScreenContent;
    [SerializeField]
    private Image LoadingScreenFade;
    private Color LoadingScreenColor;

    private float FadeTimer = 0.5f;

    private UILoadingProgressCore[] ProgressComps;
    private UILoadingTextCore[] TextComps;
    
    
    private void Awake()
    {
        if(!Implement(this, out var err))
        {
            Debug.LogError(err);
        }
        LoadingScreenColor = LoadingScreenFade.color;
        LoadingScreenFade.gameObject.SetActive(false);
        
        ProgressComps = GetComponentsInChildren<UILoadingProgressCore>();
        TextComps = GetComponentsInChildren<UILoadingTextCore>();
    }

    /// <summary>
    /// Toggles the loading screen
    /// </summary>
    /// <param name="show">bool if to show the loadingscreen or not</param>
    public void ToggleLoadingScreen(bool show)
    {
        LoadingScreenContent.SetActive(show);
        LoadingScreenFade.gameObject.SetActive(false);

        foreach (var tc in TextComps)
        {
            tc.Reset();
        }
        foreach (var pc in ProgressComps)
        {
            pc.SetProgress(0f);
        }
    }
    
   
    public void FadeOutLoadingScreen(Action onDone)
    {
        LoadingScreenFade.gameObject.SetActive(true);
        LoadingScreenFade.color = LoadingScreenColor;
        LoadingScreenContent.SetActive(false);
        
        StartCoroutine(FadeOut(onDone));
    }
    

    /// <summary>
    /// If text component is available, sets the text
    /// </summary>
    /// <param name="text"></param>
    public void SetLoadingScreenText(string text, LOADINGSCREEN_TEXT_TYPE type = LOADINGSCREEN_TEXT_TYPE.UNDEFINED)
    {
        var tc = TextComps.FirstOrDefault(t => t.GetTextType == type);
        if (tc != null)
        {
            tc.SetText(text);
        }
    }
    
    /// <summary>
    /// Sets the progress if available
    /// </summary>
    /// <param name="progress"></param>
    public void SetLoadingScreenProgress(float progress)
    {
        foreach (var pc in ProgressComps)
        {
            pc.SetProgress(progress);
        }
    }

    private IEnumerator FadeOut(Action onDone)
    {
        float startAlpha = 1f;
        float time = 0f;
        var color = LoadingScreenColor;

        while (time < FadeTimer)
        {
            time += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, 0f, time / FadeTimer);
            LoadingScreenFade.color = color;
            yield return null;
        }

        color.a = 0f;
        LoadingScreenFade.color = color;
        ToggleLoadingScreen(false);
        onDone?.Invoke();
    }
}
