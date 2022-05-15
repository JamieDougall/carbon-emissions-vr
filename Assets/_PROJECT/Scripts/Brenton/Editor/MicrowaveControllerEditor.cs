using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MicrowaveController))]
public class MicrowaveControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        MicrowaveController mc = target as MicrowaveController;
        if (GUILayout.Button("Start"))
        {
            mc.StartMicrowave();
        }
    }
}
