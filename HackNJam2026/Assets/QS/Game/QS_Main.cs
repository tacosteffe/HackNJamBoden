using System;
using UnityEngine;

/// <summary>
/// Main script file for QS
/// Will handle init of systems which will later handle the game
/// </summary>
[DisallowMultipleComponent]
[
    RequireComponent(typeof(GameEventManager)), 
    RequireComponent(typeof(InputManager)), 
    RequireComponent(typeof(GameMaster)),
]
public class QS_Main : Singleton<QS_Main>
{
    
    [Space]
    private GameEventManager GameEventManager;
    private InputManager InputManager;
    private GameMaster GameMaster;

    /// <summary>
    /// Called once on awake
    /// </summary>
    void Awake()
    {
        if (!Implement(this, out var err))
        {
            Debug.LogError(err);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        GameEventManager = GetComponent<GameEventManager>();
        GameEventManager.Initialize();

        InputManager = GetComponentInChildren<InputManager>();
        InputManager.Initialize();
        
        //Final init is the game master, this will launch the gamestates and manage them
        GameMaster = GetComponentInChildren<GameMaster>();
        GameMaster.Initialize();
    }

    private void Clean()
    {
        InputManager.Clean();
    }
}
