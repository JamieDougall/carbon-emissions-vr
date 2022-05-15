using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataController))]
public class DataControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        DataController dc = target as DataController;
        if (GUILayout.Button("Load Data"))
        {
            dc.LoadData();
        }
    }
}
