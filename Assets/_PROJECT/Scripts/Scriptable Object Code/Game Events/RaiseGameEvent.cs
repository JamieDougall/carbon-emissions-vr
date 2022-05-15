using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaiseGameEvent : MonoBehaviour
{
    public GameEvent gameEvent;

    public void CallGameEvent(){ gameEvent.Raise(); }
}
