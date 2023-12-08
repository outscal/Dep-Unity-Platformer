using Platformer.Enemy;
using UnityEngine;

namespace Platformer.Player{
    public class MeleeController{
        public MeleeScriptableObject meleeScriptableObject { get; private set; }
        private MeleeView meleeView;

        public MeleeController(MeleeScriptableObject meleeScriptableObject, Transform parentTransform){
            this.meleeScriptableObject = meleeScriptableObject;
            InitializeView(parentTransform);
        }

        private void InitializeView(Transform parentTransform)
        {
            meleeView = Object.Instantiate(meleeScriptableObject.Prefab, parentTransform);
            meleeView.SetController(this);
        }

        public void EnemyHit(EnemyView enemy) => enemy.TakeDamage(meleeScriptableObject.Damage);
    }
}