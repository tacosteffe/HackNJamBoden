using System;
using UnityEngine;

public class MenuSwitcher : Singleton<MenuSwitcher>
{
    [SerializeField]
    private GameEventSO GameStateChange;

    [SerializeField]
    private GameObject GameParent;


    private void Awake()
    {
        Implement(this, out var _);
        GameParent.SetActive(false);
        GameStateChange.OnEventRaised += OnStateSwitch;
    }

    private void OnDestroy()
    {
        GameStateChange.OnEventRaised += OnStateSwitch;
    }

    void OnStateSwitch(Component comp, object value)
    {
        if (value is GAMESTATE state)
        {
            if (state == GAMESTATE.GAME)
            {
                GameParent.SetActive(true);
            }
            else
            {
                GameParent.SetActive(false);
            }
        }
    }
    
}
