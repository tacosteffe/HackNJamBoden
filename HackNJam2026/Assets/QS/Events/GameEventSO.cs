using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "NewEvent", menuName = "QS/GameEvent/New Event")]
public class GameEventSO : ScriptableObject
{
    [Header("Cannot be null and should not be a duplicate of another")]
    public string EventName;
    public UnityAction<Component, object> OnEventRaised;

    public void Raise(Component component, object value)
    {
#if UNITY_EDITOR
        //Debug.Log($"EVENT RAISED FROM: {component.GetType().ToString()} - {EventName}");
#endif
        OnEventRaised?.Invoke(component, value);
    }
}
