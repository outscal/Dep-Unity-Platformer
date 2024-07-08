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

        public EnemyType enemyType;
        public float PatrollingSpeed;

        public Dictionary<EnemyType, EnemyScriptableObject> enemyConfigurations;
        
        
        
        
        [Header("HEALTH SETTINGS")]
        public int MaximumHealth;
        public float HealthbarPositionOffset;

        [Header("COMBAT SETTINGS")]
        public float AttackRangeRadius;
        public int DamageToInflict;
        public MeleeScriptableObject MeleeSO;
        public float DelayAfterAttack;

        [Header("MISCELLANEOUS SETTINGS")]
        public float DelayAfterGameEnd;
    }
}