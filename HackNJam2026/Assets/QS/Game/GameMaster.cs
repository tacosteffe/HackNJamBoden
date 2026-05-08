using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : Singleton<GameMaster>
{
    [SerializeField, Header("Starting gamestate")]
    private GAMESTATE StartingState = GAMESTATE.MENU;
    
    [SerializeField, Space(20f)]
    private GameEventSO StartGameEvent;
    [SerializeField]
    private GameEventSO PauseGameEvent;
    [SerializeField]
    private GameEventSO ExitGameEvent;
    [SerializeField]
    private GameEventSO AppFocusEvent;
    
    
    [SerializeField]
    private GameEventSO GameStateChangeEvent;

    private GAMESTATE CurrentState = GAMESTATE.INIT;
    
    void Awake()
    {
        if (!Implement(this, out var err))
        {
            Debug.LogError(err);
        }
    }

    public void Initialize()
    {
        StartGameEvent.OnEventRaised += StartGame;
        PauseGameEvent.OnEventRaised += PauseGame;
        ExitGameEvent.OnEventRaised += ExitGame;
 
        GameSceneManager.Instance.LoadScene(
            StartingState, 
            LoadSceneMode.Additive, 
            false, 
            () => SwitchState(StartingState), 
            false);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        AppFocusEvent.Raise(this, hasFocus);
    }

    
    
    #region Events
    

    public void StartGame(Component comp, object val)
    {
        BeginSwitch();
        GameSceneManager.Instance.LoadScene(
            GAMESTATE.GAME, 
            LoadSceneMode.Additive, 
            true, 
            () => EndSwitch(GAMESTATE.GAME));
    }
    
    
    public void PauseGame(Component comp, object val)
    {
        SwitchState(CurrentState == GAMESTATE.GAME ? GAMESTATE.PAUSE : GAMESTATE.GAME);
    }
    
    public void ExitGame(Component comp, object val)
    {
        BeginSwitch();
        GameSceneManager.Instance.LoadScene(
            GAMESTATE.MENU, 
            LoadSceneMode.Additive, 
            true, 
            () => EndSwitch(GAMESTATE.MENU), 
            false);
    }

    
    #endregion
    
    
    
    #region State switching

    
    /// <summary>
    /// Switches state immediately
    /// </summary>
    /// <param name="newState"></param>
    public void SwitchState(GAMESTATE newState)
    {
        if (CurrentState == newState || CurrentState == GAMESTATE.SWITCHING)
            return;

        CurrentState = newState;
        GameStateChangeEvent.Raise(this, CurrentState);
    }

    /// <summary>
    /// Begins a gamestate switch and hinders other gamestate changes
    /// Usefull when waiting for something like a scene change
    /// </summary>
    public void BeginSwitch()
    {
        CurrentState = GAMESTATE.SWITCHING;
    }
    
    /// <summary>
    /// Ends a gamestate switch
    /// </summary>
    public void EndSwitch(GAMESTATE newState)
    {
        if (CurrentState != newState && CurrentState == GAMESTATE.SWITCHING)
        {
            CurrentState = newState;
            GameStateChangeEvent.Raise(this, CurrentState);
        }
    }
    
    
    #endregion
    
    
    
    #region jam specifics

    public void StartGameExt()
    {
        StartGame(this, null);
    }
    
    public void PauseGameExt()
    {
        PauseGame(this, null);
        
    }
    
    public void ExitGameExt()
    {
        ExitGame(this, null);
    }
    
    #endregion
}

public enum GAMESTATE
{
    //Initial state, this should never be called otherwise
    INIT = 0,

    //Menu state is the main menu
    MENU,

    //Game state
    GAME,
    
    //Pause state
    PAUSE,
    
    //Gameover state
    GAMEOVER,
    
    //Internal states
    SWITCHING,
    COUNT
}