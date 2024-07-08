using System.Collections;
using System.Collections.Generic;
using Platformer.Enemy;
using UnityEngine;

namespace Platformer.Level
{
    [CreateAssetMenu(fileName = "LevelEnemySpawnConfigSO", menuName = "ScriptableObjects/LevelEnemySpawnConfigSO")]
    public class LevelEnemySpawnConfigSO : ScriptableObject
    {
        public List<EnemySpawnData> enemySpawnDataList;
       
    }
}