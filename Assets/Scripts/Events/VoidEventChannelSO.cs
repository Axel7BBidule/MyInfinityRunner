using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/Void Events Channel", fileName = "voidEventsChannel_SO")]
public class VoidEventsChannel : ScriptableObject
{
    public UnityAction OnEventRaised;

    public void RaiseEvent()
    {
        OnEventRaised?.Invoke();
    }

}
