using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : Singleton<GameSceneManager>
{
    [SerializeField] 
    private string PersistentSceneName = "PersistentScene";
    [SerializeField] 
    private List<SceneWrapper> SceneWrappers;
    private bool IsSwitching = false;


    private void Awake()
    {
        if(!Implement(this, out var err))
        {
            Debug.LogError(err);
        }
    }
    
    /// <summary>
    /// Only use unload previous if using additive scene loading/unloading
    /// </summary>
    /// <param name="sceneName"></param>
    /// <param name="loadMode"></param>
    /// <param name="unloadPrevious"></param>
    /// <param name="onDone"></param>
    public void LoadScene(GAMESTATE state, LoadSceneMode loadMode, bool unloadPrevious = true, Action onDone = null, bool fadeOut = true)
    {
        if(IsSwitching)
        {
            Debug.LogWarning($"Can't load new scene for state: {state.ToString()}, other scene is not done loading yet");
        }

        var stateScene = SceneWrappers.FirstOrDefault(s => s.State == state);

        if (string.IsNullOrEmpty(stateScene.SceneName))
        {
            Debug.LogError($"No scene / scene name for state: {state.ToString()}");
            return;
        }
        
        StartCoroutine(LoadUnloadScene(stateScene.SceneName, loadMode, unloadPrevious, onDone, fadeOut));
    }



    private IEnumerator LoadUnloadScene(string newSceneName, LoadSceneMode loadMode, bool unloadPrevious, Action onDone = null, bool fadeOut = true)
    {
        IsSwitching = true;
        UILoadingScreen.Instance.ToggleLoadingScreen(true);
        
        yield return new WaitForSecondsRealtime(0.3f);

        var currentScene = SceneManager.GetActiveScene();

        //Check if unload is neccessary
        if ((currentScene.name != newSceneName || currentScene.name != PersistentSceneName) && unloadPrevious)
        {
            Debug.Log($"Unloading scene: {currentScene.name}");

            var op = SceneManager.UnloadSceneAsync(currentScene.name, UnloadSceneOptions.None);

            UILoadingScreen.Instance.SetLoadingScreenText("Unloading previous scene..", LOADINGSCREEN_TEXT_TYPE.MAIN);

            int breakOut = 0;
            while (op.progress < 1f)
            {
                yield return new WaitForSecondsRealtime(.1f);
                UILoadingScreen.Instance.SetLoadingScreenProgress(op.progress);
                
                if (breakOut > 100)
                {
                    Debug.LogError($"Breakout called for unloading scene with index: {currentScene.buildIndex}");
                    break;
                }

                breakOut++;
            }
        }

        //Check if load is neccessary
        if (currentScene.name != newSceneName)
        {
            Debug.Log($"Unloading scene: {newSceneName}");

            var op = SceneManager.LoadSceneAsync(newSceneName, loadMode);

            UILoadingScreen.Instance.SetLoadingScreenText("Loading scene..", LOADINGSCREEN_TEXT_TYPE.MAIN);

            int breakOut = 0;
            while (op.progress < 1f)
            {
                yield return new WaitForSecondsRealtime(.1f);
                UILoadingScreen.Instance.SetLoadingScreenProgress(op.progress);

                if (breakOut > 100)
                {
                    Debug.LogError($"Breakout called for Loading scene with index: {currentScene.buildIndex}");
                    break;
                }

                breakOut++;
            }
        }

        yield return new WaitForSecondsRealtime(.3f);

        if(!SceneManager.SetActiveScene(SceneManager.GetSceneByName(newSceneName)))
            Debug.LogError($"Failed to set new scene: {newSceneName} as active");
        
        IsSwitching = false;
        if (fadeOut)
        {
            UILoadingScreen.Instance.FadeOutLoadingScreen(onDone);
        }
        else
        {
            UILoadingScreen.Instance.ToggleLoadingScreen(false);
            onDone?.Invoke();            
        }
    }

    [System.Serializable]
    private struct SceneWrapper
    {
        public string SceneName;
        public GAMESTATE State;
    }
}
