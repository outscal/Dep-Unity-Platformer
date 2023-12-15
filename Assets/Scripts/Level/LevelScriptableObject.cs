using System.Collections.Generic;
using Platformer.Drop;
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
        public List<DropScriptableObject> DropScriptableObjects;
    }
}