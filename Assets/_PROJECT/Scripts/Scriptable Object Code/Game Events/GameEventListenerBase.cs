using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Based on code by @roboryantron - https://github.com/roboryantron/Unite2017
/// </summary>
public class GameEventListenerBase : MonoBehaviour
{
    [HideInInspector]
    public GameEvent Event;
    [HideInInspector]
    public UnityEvent Response;

    public void AddNewEvent(GameEvent newEvent)
    {
        if(Event == null)
        {
            Event = newEvent;
            Event.RegisterListener(this);
        }
    }

    /// <summary> UID is used to identify game events of the same name </summary>
    public virtual void OnEventRaised(string eventUID)
    { Response.Invoke(); }
}
