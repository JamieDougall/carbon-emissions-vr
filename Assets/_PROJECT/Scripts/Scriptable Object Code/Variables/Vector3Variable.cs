using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Vector3 Variable")]
public class Vector3Variable : ScriptableObject, ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

    public Vector3 InitialValue;

    [NonSerialized]
    public Vector3 RuntimeValue;

    public void OnAfterDeserialize()
    {
        RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() { }
}
