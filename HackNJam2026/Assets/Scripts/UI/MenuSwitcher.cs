using System;
using UnityEngine;

public class MenuSwitcher : MonoBehaviour
{
    [SerializeField]
    private GameEventSO GameStateChange;

    [SerializeField]
    private GameObject MenuParent;
    [SerializeField]
    private GameObject GameParent;


    private void Awake()
    {
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
            if (state == GAMESTATE.MENU)
            {
                MenuParent.SetActive(true);
                GameParent.SetActive(false);
            }
            else if (state == GAMESTATE.GAME)
            {
                MenuParent.SetActive(false);
                GameParent.SetActive(true);
            }
            else
            {
                MenuParent.SetActive(false);
                GameParent.SetActive(false);
            }
        }
    }
    
}
