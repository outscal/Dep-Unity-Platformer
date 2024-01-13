using UnityEngine;

namespace Platformer.Cameras
{
    [CreateAssetMenu(fileName = "CameraScriptableObject", menuName = "ScriptableObjects/CameraScriptableObject")]
    public class CameraScriptableObject : ScriptableObject
    {
        public CameraView Prefab;
    }
}