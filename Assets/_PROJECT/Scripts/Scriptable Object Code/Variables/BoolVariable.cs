using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Bool Variable")]
public class BoolVariable : ScriptableObject, ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

	public bool InitialValue;

	[NonSerialized]
	public bool RuntimeValue;

public void OnAfterDeserialize()
{
		RuntimeValue = InitialValue;
}

public void OnBeforeSerialize() { }

}
