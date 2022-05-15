using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Vector2 Variable")]
public class Vector2Variable : ScriptableObject, ISerializationCallbackReceiver
{

    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public Vector2 InitialValue;

	[NonSerialized]
	public Vector2 RuntimeValue;

    public void OnAfterDeserialize()
    {
		RuntimeValue = InitialValue;
    }

    public void OnBeforeSerialize() { }
}
