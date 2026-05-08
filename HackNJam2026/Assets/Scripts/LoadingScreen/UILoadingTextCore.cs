using UnityEngine;

/// <summary>
/// Core component for text widgets for the loading screen
/// </summary>
public abstract class UILoadingTextCore : MonoBehaviour
{
    protected LOADINGSCREEN_TEXT_TYPE  TextType;
    public LOADINGSCREEN_TEXT_TYPE GetTextType => TextType;
    
    /// <summary>
    /// Resets the component
    /// </summary>
    public abstract void Reset();
    
    /// <summary>
    /// Sets the text for the loading screen
    /// </summary>
    /// <param name="text">text to show</param>
    public abstract void SetText(string text);
}

public enum LOADINGSCREEN_TEXT_TYPE
{
    UNDEFINED = 0,
    MAIN,
    DESCRIPTION,
    INFO,
    MISC1,
    MISC2,
    MISC3,
}