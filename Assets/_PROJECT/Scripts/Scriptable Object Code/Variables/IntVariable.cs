using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Variables/Int Variable")]
public class IntVariable : ScriptableObject, ISerializationCallbackReceiver
{

#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif

	public int InitialValue;

	[NonSerialized]
	public int RuntimeValue;

public void OnAfterDeserialize()
{
	RuntimeValue = InitialValue;
}

public void OnBeforeSerialize() { }


}
