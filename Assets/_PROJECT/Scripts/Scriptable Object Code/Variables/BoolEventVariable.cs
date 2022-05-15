using UnityEngine;

[CreateAssetMenu(menuName = "Events/Bool Event Variable")]
public class BoolEventVariable : ScriptableObject, ISerializationCallbackReceiver
{
    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public bool InitialValue;
    [SerializeField]
    private bool _myBool;
    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate();

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize(){ Bool = InitialValue; }
    public bool Bool
    {
        get
        {
            return _myBool;
        }
        set
        {
            if (_myBool == value) return;
            _myBool = value;
            if (OnVariableChange != null)
                OnVariableChange();
        }
    }
}
