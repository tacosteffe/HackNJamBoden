
using System.Runtime.CompilerServices;

/// <summary>
/// Looping timer, returns true when loop is done and resets itself.
/// Use EasyTimerNL if you want Delta for the timer value
/// </summary>
[System.Serializable]
public struct EasyTimer
{
    float Time;
    float TimerMax;

    /// <summary>
    /// Get the current time for the timer
    /// </summary>
    public float GetTime => Time;

    /// <summary>
    /// Get the current delta of the timer (0-1)
    /// </summary>
    public float GetDelta => Time / TimerMax;

    public EasyTimer(float timeMax, float initialTime = 0f) => (TimerMax, Time) = (timeMax, initialTime);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Update(float dt)
    {
        if ((Time += dt) >= TimerMax)
        {
            Time -= TimerMax;
            return true;
        }
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ResetTimer() => Time = 0f;
}


/// <summary>
/// Non looping timer, returns delta of the time / timeMax
/// </summary>
[System.Serializable]
public struct EasyTimerNL
{
    float Time;
    float TimerMax;

    /// <summary>
    /// Get the current time for the timer
    /// </summary>
    public float GetTime => Time;

    public EasyTimerNL(float timeMax, float initialTime = 0f) => (TimerMax, Time) = (timeMax, initialTime);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public float Update(float dt)
    {
        return (Time += dt) / TimerMax;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void ResetTimer() => Time = 0f;
}