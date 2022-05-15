using UnityEngine;

[CreateAssetMenu(menuName = "Containers/Rigidbody")]
public class RigidbodyHolder : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public Rigidbody rigidbody = null;

    public void Clear(){ rigidbody = null; }
}
