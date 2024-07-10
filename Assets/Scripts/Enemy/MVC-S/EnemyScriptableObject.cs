using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "EnemyScriptableObject", menuName = "ScriptableObjects/EnemyScriptableObject")]
    public class EnemyScriptableObject : ScriptableObject
    {
        [Header("GENERAL SETTINGS")]
        public EnemyView Prefab;

        public EnemyType enemyType;

        public Dictionary<EnemyType, EnemyScriptableObject> enemyConfigurations;
        
        [Header("HEALTH SETTINGS")]
        public int MaximumHealth;
        public float HealthbarPositionOffset;

        [Header("COMBAT SETTINGS")]
        public float AttackRangeRadius;
        public float DelayAfterAttack;
        
        [Header("DAMAGE SETTINGS")]
        public int DamageToInflict;

        [Header("MISCELLANEOUS SETTINGS")]
        public float DelayAfterGameEnd;
    }
}