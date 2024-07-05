using Platformer.Player;
using UnityEngine;

namespace Platformer.AnimationSystem
{
    public class AnimationService
    {
        private Animator playerAnimator;

        public AnimationService(Animator playerAnimator) 
        { 
            this.playerAnimator = playerAnimator;
        }

        public void PlayPlayerMovementAnimation(PlayerState currentPlayerState)
        {
            playerAnimator.SetBool(PlayerAnimationType.RUNNING.ToString(), currentPlayerState == PlayerState.RUNNING);
        }

        public void PlayPlayerTriggerAnimation(PlayerTriggerAnimationType animationToPlay)
        {
            playerAnimator.SetTrigger(animationToPlay.ToString());
        }
    }
}