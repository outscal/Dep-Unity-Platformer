using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "MovableEnemyScriptableObject", menuName = "ScriptableObjects/MovableEnemyScriptableObject")]
    public class MovableEnemyScriptableObject : EnemyScriptableObject
    {
        [Header("PATROLLING SETTINGS")]
        public float PatrollingSpeed;
    }
}