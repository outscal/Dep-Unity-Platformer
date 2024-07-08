using UnityEngine;

namespace Platformer.Melee{
    [CreateAssetMenu(fileName = "MeleeScriptableObject", menuName = "ScriptableObjects/MeleeScriptableObject")]
    public class MeleeScriptableObject : ScriptableObject
    {
        // public MeleeView Prefab;
        public int DamageToInflict;
        public float MeleeDuration;
    }
}