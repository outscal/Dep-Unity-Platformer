using UnityEngine;

namespace Platformer.AnimationSystem{
    public class AnimationService{

        #region Player Animations

        #region Player Animator Parameters
        private const string IS_RUNNING = "Running";
        private const string JUMP = "Jump";
        private const string TAKE_DAMAGE = "TakeDamage";
        private const string DEATH = "Death";
        private const string ATTACK = "Attack";
        private const string SLIDE = "Slide";
        #endregion

        public void PlayPlayerMovementAnimation(Animator playerAnimator, bool isRunning) => playerAnimator.SetBool(IS_RUNNING, isRunning);
        public void PlayPlayerJumpAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(JUMP);
        public void PlayPlayerDamageAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(TAKE_DAMAGE);
        public void PlayPlayerDeathAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(DEATH);
        public void PlayPlayerAttackAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(ATTACK);
        public void PlayPlayerSlideAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(SLIDE);

        #endregion
    }
}