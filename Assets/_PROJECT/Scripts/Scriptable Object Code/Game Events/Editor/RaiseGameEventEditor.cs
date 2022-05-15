using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RaiseGameEvent))]
public class RaiseGameEventEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        RaiseGameEvent rge = target as RaiseGameEvent;
        if (GUILayout.Button("Raise!"))
        {
            rge.CallGameEvent();
        }
    }
}
