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
        public int maxHealth;
        public int delayAfterDeath;
    }
}