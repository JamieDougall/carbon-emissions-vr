using UnityEngine;

[CreateAssetMenu(menuName = "Containers/GameObject")]
public class GameobjectHolder : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public GameObject gameObject = null;

    public void Clear(){ gameObject = null; }
}
