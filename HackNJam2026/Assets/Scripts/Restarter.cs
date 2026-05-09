using System;
using UnityEngine;

public class Restarter : MonoBehaviour
{
    public GameEventSO GameStateChange;
    public GameEventSO StartGameEvent;
    public GameEventSO RestartGameEvent;
    private bool WaitingForRestart = false;


    private void Awake()
    {
        GameStateChange.OnEventRaised += StateChange;
    }

    public void Restart()
    {
        WaitingForRestart = true;
        RestartGameEvent.Raise(this, null);
    }

    void StateChange(Component comp, object value)
    {
        if (value is GAMESTATE state)
        {
            if (state == GAMESTATE.MENU && WaitingForRestart)
            {
                StartGameEvent.Raise(this, null);
                WaitingForRestart = false;
            }
        }
    }
}
