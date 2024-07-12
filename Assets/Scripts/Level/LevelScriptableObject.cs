using Platformer.Cameras;
using UnityEngine;

namespace Platformer.Level
{
    [CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "ScriptableObjects/LevelScriptableObject")]
    public class LevelScriptableObject : ScriptableObject
    {
        public int ID;
        public LevelView LevelPrefab;
        public CameraBounds CameraBounds;
    }
}