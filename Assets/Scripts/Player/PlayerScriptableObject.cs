using UnityEngine;

namespace Platformer.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        public PlayerView prefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        public float movementSpeed;
    }
}