using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Melee
{
    public class MeleeController
    {
        public MeleeScriptableObject MeleeScriptableObject { get; private set; }
        private MeleeView meleeView;

        public MeleeController(MeleeScriptableObject meleeScriptableObject, Transform parentTransform)
        {
            MeleeScriptableObject = meleeScriptableObject;
            InitializeView(parentTransform);
        }

        private void InitializeView(Transform parentTransform)
        {
            meleeView = Object.Instantiate(MeleeScriptableObject.Prefab, parentTransform);
            meleeView.SetController(this);
        }

        public void InflictDamage(Collider2D other) => other.GetComponent<IDamagable>()?.TakeDamage(MeleeScriptableObject.DamageToInflict);
    }
}