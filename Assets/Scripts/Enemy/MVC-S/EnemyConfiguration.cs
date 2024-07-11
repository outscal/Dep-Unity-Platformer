using System;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfiguration", menuName = "ScriptableObjects/EnemyConfiguration")]
    public class EnemyConfiguration : ScriptableObject
    {
        [Serializable]
        public struct EnemyTypeToSO
        {
            public EnemyType enemyType;
            public EnemyScriptableObject enemyScriptableObject;
        }
        
        public List<EnemyTypeToSO> enemyConfigurations = new List<EnemyTypeToSO>();
    }
}