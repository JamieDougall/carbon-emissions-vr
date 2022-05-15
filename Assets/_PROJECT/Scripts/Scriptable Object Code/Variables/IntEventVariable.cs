using UnityEngine;

[CreateAssetMenu(menuName = "Events/Int Event Variable")]
public class IntEventVariable : ScriptableObject, ISerializationCallbackReceiver
{

    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public int InitialValue;
    [SerializeField]
    private int _myInt;
    public event OnVariableChangeDelegate OnVariableChange;
    public delegate void OnVariableChangeDelegate();

    public void OnBeforeSerialize(){}
    public void OnAfterDeserialize(){ Int = InitialValue; }
    public int Int
    {
        get
        {
            return _myInt;
        }
        set
        {
            if (_myInt == value) return;
            _myInt = value;
            if (OnVariableChange != null)
                OnVariableChange();
        }
    }

}