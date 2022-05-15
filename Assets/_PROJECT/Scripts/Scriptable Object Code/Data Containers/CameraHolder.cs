using UnityEngine;


[CreateAssetMenu(menuName = "Containers/Camera")]
public class CameraHolder : ScriptableObject
{
    #if UNITY_EDITOR
    [Multiline]
    public string developerDescription = "";
    #endif

    public Camera currentCamera = null;
}
