#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    #if UNITY_EDITOR
    void Update()
    {
        if (Application.isPlaying)
        {
            Transform sceneCameraTransform = SceneView.lastActiveSceneView.camera.transform;
            transform.position = sceneCameraTransform.position;
            transform.rotation = sceneCameraTransform.rotation;
        }
    }
    #endif
}
