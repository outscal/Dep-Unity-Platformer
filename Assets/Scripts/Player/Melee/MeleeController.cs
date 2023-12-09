using Platformer.Enemy;
using Platformer.Main;
using Platformer.Sound;
using UnityEngine;

namespace Platformer.Player{
    public class MeleeController{
        #region Service References
        private SoundService SoundService => GameService.Instance.SoundService;
        #endregion

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

        public void EnemyHit(EnemyView enemy){
            SoundService.PlaySoundEffects(SoundType.PLAYER_ATTACK);
            enemy.TakeDamage(meleeScriptableObject.Damage);
        }
    }
}