using System.Collections.Generic;
using Platformer.Enemy;
using UnityEngine;

namespace Platformer.Level
{
    [CreateAssetMenu(fileName = "LevelScriptableObject", menuName = "ScriptableObjects/LevelScriptableObject")]
    public class LevelScriptableObject : ScriptableObject
    {
        public int ID;
        public LevelView LevelPrefab;
        public List<EnemyScriptableObject> EnemyScriptableObjects;
        public LevelEnemySpawnConfigSO EnemySpawnConfig;
    }
}