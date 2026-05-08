using UnityEngine;

/// <summary>
/// Core component for LoadingScreen progress widgets
/// </summary>
public abstract class UILoadingProgressCore : MonoBehaviour
{

    /// <summary>
    /// Resets the component
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// Sets the progress
    /// </summary>
    /// <param name="progress">progress between 0-1</param>
    public abstract void SetProgress(float progress);
}
