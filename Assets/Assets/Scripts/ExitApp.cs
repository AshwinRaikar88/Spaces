using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ExitApp : MonoBehaviour
{
    public void Exit()
    {
    #if UNITY_EDITOR
        // Stop play mode in the Unity Editor
        EditorApplication.isPlaying = false;
    #else
        // Quit the built application
        Application.Quit();
    #endif
    }
}
