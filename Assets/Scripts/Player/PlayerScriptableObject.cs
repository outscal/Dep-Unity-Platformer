using UnityEngine;

namespace Platformer.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        // Initialization Data:
        public PlayerView prefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        
        // Player Attributes:
        public int maxHealth;
        public float movementSpeed;
        public float slidingSpeed;
        public float slidingTime;
        public float jumpForce;
        public int delayAfterDeath;
        
        // TODO: What the fuck are these multipliers even supposed to be ??!! :D 
        // Multipliers:
        public float gravityDownForceMultiplier;
        public float fallMultiplier;
        public float lowJumpMultiplier;
    }
}