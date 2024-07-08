using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [System.Serializable]
    public struct EnemySpawnData
    {
        public EnemyType EnemyType;
        public Vector3 SpawnPosition;
        public List<Vector3> PatrolPoints; 
    }
}