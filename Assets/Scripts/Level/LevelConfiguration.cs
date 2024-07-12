using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Level
{
    [CreateAssetMenu(fileName = "LevelConfiguration", menuName = "ScriptableObjects/LevelConfiguration")]
    public class LevelConfiguration: ScriptableObject
    {
        public List<LevelScriptableObject> levelConfig;
    }
}