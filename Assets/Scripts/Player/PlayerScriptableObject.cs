using Platformer.Melee;
using UnityEngine;

namespace Platformer.Player
{
    [CreateAssetMenu(fileName = "PlayerScriptableObject", menuName = "ScriptableObjects/PlayerScriptableObject")]
    public class PlayerScriptableObject : ScriptableObject
    {
        public PlayerView prefab;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        public MeleeScriptableObject meleeSO;
        public float movementSpeed;
        public float slidingSpeed;
        public float slidingTime;
        public float jumpForce;
        public float gravityDownForceMultiplier;
        public float fallMultiplier;
        public float lowJumpMultiplier;
        public int maxHealth;
        public int delayAfterDeath;
    }
}