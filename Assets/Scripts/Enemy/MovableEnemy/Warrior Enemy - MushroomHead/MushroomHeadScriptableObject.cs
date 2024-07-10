using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "MushroomHeadScriptableObject", menuName = "ScriptableObjects/MushroomHeadScriptableObject")]
    public class MushroomHeadScriptableObject : MovableEnemyScriptableObject
    {
        public LayerMask AttackTargetLayer; 
    }
}