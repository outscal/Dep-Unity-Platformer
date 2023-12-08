using UnityEngine;

namespace Platformer.Player{
    [CreateAssetMenu(fileName = "MeleeScriptableObject", menuName = "ScriptableObjects/MeleeScriptableObject")]
    public class MeleeScriptableObject : ScriptableObject
    {
        public MeleeView Prefab;
        public int Damage;
        public float MeleeDuration;
    }
}