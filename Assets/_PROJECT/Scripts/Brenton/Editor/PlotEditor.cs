using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CustomEditor(typeof(Plot))]
public class PlotEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Plot plot = target as Plot;
        if (GUILayout.Button("Draw Line Plot"))
        {
            plot.DrawLinePlot();
        }
        if (GUILayout.Button("Draw Scatter Plot"))
        {
            plot.DrawScatterPlot();
        }
        if (GUILayout.Button("Draw Bar Plot"))
        {
            plot.DrawBarPlot();
        }
        if (GUILayout.Button("Dummy Values"))
        {
            plot.DummyValues();
            plot.DrawBarPlot();
        }
    }
}
