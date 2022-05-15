using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Based on code by @roboryantron - https://github.com/roboryantron/Unite2017
/// </summary>
public class GameEventListener : GameEventListenerBase
{
    [Tooltip("Event to register with")]
    public GameEvent gameEvent;
    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent eventResponse;

    private void OnEnable()
    {
        if(gameEvent != null && eventResponse != null)
        {
            Event = gameEvent;
            Response = eventResponse;

            Event.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if(Event != null)
            Event.UnregisterListener(this);
    }
}
