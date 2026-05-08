using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : class
{
    public static T Instance {  get; private set; }


    /// <summary>
    /// Implements the singleton
    /// </summary>
    /// <param name="instance"></param>
    protected bool Implement(T instance, out string error)
    {
        if(Instance == null)
        {
            Instance = instance;
            error = "";
            return true;
        }
        else
        {
            error = $"Trying to implement a second singleton of type {typeof(T)}";
            return false;
        }
    }

    /// <summary>
    /// Implements the singelton, overwriting the instance if there is one
    /// </summary>
    /// <param name="instance"></param>
    protected void ImplementForce(T instance)
    {
        Instance = instance;
    }


    /// <summary>
    /// Removes the implement of this singleton
    /// </summary>
    protected virtual void OnDestroy()
    {
        Instance = null;
    }

}
