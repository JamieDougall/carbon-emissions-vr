using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using JetBrains.Annotations;

[CustomEditor(typeof(CarEmissions))]
public class CarEmissionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        CarEmissions ce = target as CarEmissions;
        if (GUILayout.Button("Load Makes"))
        {
            ce.LoadMakes();
        }
        if (GUILayout.Button("Load Models"))
        {
            ce.LoadModels();
        }
        if (GUILayout.Button("Load Years"))
        {
            ce.LoadYears();
        }
        if (GUILayout.Button("Get KM Per Litre"))
        {
            ce.GetKMPerLitre();
        }
        if (GUILayout.Button("Draw Plot"))
        {
            ce.DrawPlot();
        }
    }
}
