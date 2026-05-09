using System;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;



/// <summary>
/// Controller for managing input, subscribers can then subscribe to the events
/// </summary>
[DisallowMultipleComponent]
public class InputManager : Singleton<InputManager>
{

#if ENABLE_INPUT_SYSTEM

    private InputActionAsset ActionAsset;
    private string CurrentActionMap = "";

    [SerializeField]
    private GameEventSO GameStateChangeEvent;

    #region Initialization

    void Awake()
    {
        if (!Implement(this, out var err))
        {
            Debug.LogError(err);
        }
    }

    public void Initialize(string defaultMap = "UI")
    {
        ActionAsset = InputSystem.actions;
        if (ActionAsset == null)
        {
            Debug.LogError("No projectwide InputActionAsset set!");
        }
        else
        {
            var mappings = ActionAsset.actionMaps.Select(am => am.name).ToList();
            Debug.Log($"Loaded ActionAsset with Maps: {String.Join(", ", mappings)}");
        }

        if (!String.IsNullOrEmpty(defaultMap))
        {
            ChangeActionMap(defaultMap);
        }

        GameStateChangeEvent.OnEventRaised += GameStateChange;
    }

    public void Clean()
    {
        GameStateChangeEvent.OnEventRaised -= GameStateChange;
        //TODO clean input mappings?
    }



    #endregion


    #region Events

    private void GameStateChange(Component comp, object val)
    {
        if (val is GAMESTATE state)
        {
            switch (state)
            {
                case GAMESTATE.MENU:
                case GAMESTATE.PAUSE:
                default:
                    ChangeActionMap("UI");
                    break;

                case GAMESTATE.GAME:
                    ChangeActionMap("Player");
                    break;
            }
        }
    }

    #endregion


    #region Registration & Action

    /// <summary>
    /// Changes the input to a specific actionmap
    /// You can find the actionmap names in the InputActionAsset
    /// </summary>
    /// <param name="actionmap">Name of the actionmap (Case-sensitive)</param>
    public void ChangeActionMap(string actionmap)
    {
        //Empty name -> return
        if (string.IsNullOrEmpty(actionmap))
            return;

        //Check if new actionmap exists, if not return        
        var newMap = ActionAsset.FindActionMap(actionmap);
        if (newMap == null) return;

        //Disable the old action map if it exists
        if (!string.IsNullOrEmpty(CurrentActionMap))
        {
            var currentMap = ActionAsset.FindActionMap(CurrentActionMap);
            if (currentMap != null)
                currentMap.Disable();
        }

        //Enable new actionmap
        CurrentActionMap = actionmap;
        newMap.Enable();
    }



    /// <summary>
    /// Register an action that is either performed once or if you want to listen (pass through value)
    /// </summary>
    /// <param name="actionmap">The name of the action map, eg. UI, Player</param>
    /// <param name="actionName">The name of the action, eg. Jump</param>
    /// <param name="func">The function to be called</param>
    public void SubscribeSingleAction(string actionmap, string actionName, Action<InputAction.CallbackContext> func)
    {
        if (ActionAsset == null) return;
        try
        {
            var map = ActionAsset.FindActionMap(actionmap);
            var action = map.FindAction(actionName);
            action.performed += func;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Unsubscribe an action that is either performed once or if you want to listen (pass through value)
    /// </summary>
    /// <param name="actionmap">The name of the action map, eg. UI, Player</param>
    /// <param name="actionName">The name of the action, eg. Jump</param>
    /// <param name="func">The function to be called</param>
    public void UnsubscribeSingleAction(string actionmap, string actionName, Action<InputAction.CallbackContext> func)
    {
        if (ActionAsset == null) return;
        try
        {
            var map = ActionAsset.FindActionMap(actionmap);
            var action = map.FindAction(actionName);
            action.performed -= func;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Register an action that you want calls for when held and released
    /// </summary>
    /// <param name="actionmap">The name of the action map, eg. UI, Player</param>
    /// <param name="actionName">The name of the action, eg. Jump</param>
    /// <param name="func">The function to be called</param>
    public void SubscribeHeldAction(string actionmap, string actionName, Action<InputAction.CallbackContext> func)
    {
        if (ActionAsset == null) return;
        try
        {
            var map = ActionAsset.FindActionMap(actionmap);
            var action = map.FindAction(actionName);
            action.started += func;
            action.canceled += func;

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    /// <summary>
    /// Unregister an action that you want calls for when held and released
    /// </summary>
    /// <param name="actionmap">The name of the action map, eg. UI, Player</param>
    /// <param name="actionName">The name of the action, eg. Jump</param>
    /// <param name="func">The function to be called</param>
    public void UnsubscribeHeldAction(string actionmap, string actionName, Action<InputAction.CallbackContext> func)
    {
        if (ActionAsset == null) return;
        try
        {
            var map = ActionAsset.FindActionMap(actionmap);
            var action = map.FindAction(actionName);
            action.started -= func;
            action.canceled -= func;

        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    #endregion

#endif

}

