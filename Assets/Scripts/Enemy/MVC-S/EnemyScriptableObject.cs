using System.Collections.Generic;
using Platformer.Melee;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("GENERAL SETTINGS")]
        public EnemyView Prefab;
        public EnemyType Type;
        public Vector3 SpawnPosition;

        [Header("PATROLLING SETTINGS")]
        public float PatrollingSpeed;
        public List<Vector3> PatrollingPoints;

        [Header("HEALTH SETTINGS")]
        public int MaximumHealth;
        public float HealthbarPositionOffset;

        [Header("COMBAT SETTINGS")]
        public float AttackRangeRadius;
        public int DamageToInflict;
        public MeleeScriptableObject MeleeSO;
        public float DelayAfterAttack;

        [Header("MISCELLANEOUS SETTINGS")]
        public float PlayerAttackingDistance; // not in use as of now
        public float DelayAfterGameEnd;
    }
}