using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Game Event")]
/// <summary>
/// Based on code by @roboryantron - https://github.com/roboryantron/Unite2017
/// </summary>
public class GameEvent : ScriptableObject
{
    private string _uid = ""; //Assign a guid to easily log events raised
    public string uid
    {
        get {return _uid; }
        set {}
    }
    ///<summary> The list of listeners that this event will notify if it is raised. </summary>
    private List<GameEventListenerBase> listeners =
        new List<GameEventListenerBase>();

    void OnValidate()
    {
        if(String.IsNullOrEmpty(_uid))
            _uid = Guid.NewGuid().ToString();
    }
    public void Raise()
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
            listeners[i].OnEventRaised(this.uid);
    }

    public void RegisterListener(GameEventListenerBase listener)
    {
        if(String.IsNullOrEmpty(_uid))
            _uid = Guid.NewGuid().ToString();

        listeners.Add(listener); 
        //Debug.Log("listener : " + listener.name);
    }

    public void UnregisterListener(GameEventListenerBase listener)
    { listeners.Remove(listener); }
}
