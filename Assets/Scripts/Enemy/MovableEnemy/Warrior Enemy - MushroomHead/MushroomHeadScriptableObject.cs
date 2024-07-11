using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    [CreateAssetMenu(fileName = "MushroomHeadScriptableObject", menuName = "ScriptableObjects/MushroomHeadScriptableObject")]
    public class MushroomHeadScriptableObject : MovableEnemyScriptableObject
    {
        [Header("HEALTH SETTINGS")]
        public int MaximumHealth;
        
        [Header("COMBAT SETTINGS")]
        public float AttackRangeRadius;
    }
}