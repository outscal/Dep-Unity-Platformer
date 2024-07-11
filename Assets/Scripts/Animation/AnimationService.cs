using UnityEngine;

namespace Platformer.AnimationSystem{
    public class AnimationService{
        
        private const string IS_RUNNING = "Running";
        private const string JUMP = "Jump";
        private const string TAKE_DAMAGE = "TakeDamage";
        private const string DEATH = "Death";
        private const string PLAYER_ATTACK = "Attack";
        private const string SLIDE = "Slide";
  

        public void PlayPlayerMovementAnimation(Animator playerAnimator, bool isRunning) => playerAnimator.SetBool(IS_RUNNING, isRunning);
        public void PlayPlayerJumpAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(JUMP);
        public void PlayPlayerDamageAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(TAKE_DAMAGE);
        public void PlayPlayerDeathAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(DEATH);
        public void PlayPlayerAttackAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(PLAYER_ATTACK);
        public void PlayPlayerSlideAnimation(Animator playerAnimator) => playerAnimator.SetTrigger(SLIDE);



        
        private const string MUSHROOMHEAD_ATTACK = "Attack";

        public void PlayTriggerAnimation(Animator animator, string animationToPlay)
        {
            animator.SetTrigger(animationToPlay);
        }
        public void PlayMushroomHeadAttackAnimation(Animator animator) => animator.SetTrigger(MUSHROOMHEAD_ATTACK);

    }
}