using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.XR.Interaction.Toolkit;
using UnityEngine;

[CustomEditor(typeof(XRSelectiveSocket))]
public class XRSelectiveSocketEditor : XRSocketInteractorEditor
{
    private SerializedProperty targetTag = null;
    private SerializedProperty targetInteractable = null;

    protected override void OnEnable()
    {
        base.OnEnable();
        /*
        targetTag = serializedObject.FindProperty("targetTag");
        targetInteractable = serializedObject.FindProperty("targetInteractable");
        */
    }

    protected override void DrawProperties()
    {
        base.DrawProperties();
        /*
        XRSelectiveSocket socket = target as XRSelectiveSocket;
        switch (socket.mode)
        {
            case XRSelectiveSocket.SelectMode.Tag:
                EditorGUILayout.PropertyField(targetTag);
                break;
            case XRSelectiveSocket.SelectMode.Interactables:
                EditorGUILayout.PropertyField(targetTag, true);
                break;
            default:
                break;
        }
        //EditorGUILayout.PropertyField(targetInteractable);
        */
    }
}
