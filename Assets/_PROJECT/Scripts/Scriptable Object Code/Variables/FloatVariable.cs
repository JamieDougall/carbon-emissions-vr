using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Float Variable")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

	public float InitialValue;

	[NonSerialized]
	public float RuntimeValue;

public void OnAfterDeserialize()
{
		RuntimeValue = InitialValue;
}

public void OnBeforeSerialize() { }
}
