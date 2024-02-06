using System.Collections.Generic;
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
        public int MaximumHealth;
        public float RangeRadius;
        public List<Vector3> PatrollingPoints;
        public float PlayerAttackingDistance; // not in use as of now
        public float DelayAfterGameEnd;
        public float HealthbarOffset;
        public int DamageToInflict;
        public MeleeScriptableObject MeleeSO;
        public float DelayAfterAttack;
        public List<DropScriptableObject> DropScriptableObjects;
    }
}