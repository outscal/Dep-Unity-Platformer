using System.Collections.Generic;
using Platformer.Drop;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        public EnemyView Prefab;
        public EnemyType Type;
        public Vector3 SpawnPosition;
        public float PatrollingSpeed;
        public float ChasingSpeed;
        public float JumpForce;
        public int MaximumHealth;
        public float RangeRadius;
        public float RangeAngle;
        public List<Vector3> PatrollingPoints;
        public float PlayerAttackingDistance;
        public float DelayAfterGameEnd;
        public List<DropScriptableObject> DropScriptableObjects;
    }
}