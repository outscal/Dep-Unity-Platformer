using UnityEngine;

namespace Platformer.Cameras
{
    [CreateAssetMenu(fileName = "CameraScriptableObject", menuName = "ScriptableObjects/CameraScriptableObject")]
    public class CameraScriptableObject : ScriptableObject
    {
        public CameraView Prefab;

        [Header("Camera Shake")]
        public float shakeDuration;
        public float shakeMagnitude;


        [Header("Camera Zoom")]
        public float cameraSizeIncrement;
        public float maxSize;
        public float minSize;

    }
}