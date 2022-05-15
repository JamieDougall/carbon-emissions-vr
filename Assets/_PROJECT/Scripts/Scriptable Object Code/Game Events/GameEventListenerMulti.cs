using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListenerMulti : GameEventListenerBase
{
    [Tooltip("Events to register with")]
    public GameEvent[] gameEvents; //If any of these events are raised then the response will be triggered
    [Tooltip("Response to invoke when Event is raised")]
    public UnityEvent eventResponse;

    private void OnEnable()
    {
        if(gameEvents != null && eventResponse != null)
        {
            Response = eventResponse;

            foreach(GameEvent item in gameEvents)
                item.RegisterListener(this);
        }
    }

    private void OnDisable()
    {
        if(Event != null)
            Event.UnregisterListener(this);
    }
}
