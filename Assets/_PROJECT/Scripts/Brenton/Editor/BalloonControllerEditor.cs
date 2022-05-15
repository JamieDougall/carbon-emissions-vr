using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BalloonController))]
public class BalloonControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        BalloonController bc = target as BalloonController;
        if (GUILayout.Button(bc.Appliance))
        {
            bc.SpawnBalloons(bc.Appliance);
        }
        if (GUILayout.Button("Destroy Balloons"))
        {
            bc.DestroyBalloons();
        }
    }
}
