using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/String Variable")]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

	public string InitialValue;

	[NonSerialized]
	public string RuntimeValue;

public void OnAfterDeserialize()
{
		RuntimeValue = InitialValue;
}

public void OnBeforeSerialize() { }
}
