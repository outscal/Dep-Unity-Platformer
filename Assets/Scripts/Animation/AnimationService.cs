using System.Collections.Generic;
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
            playerAnimator.SetBool(PlayerAnimation.RUNNING.ToString(), currentPlayerState == PlayerState.RUNNING);
        }

        public void PlayPlayerTriggerAnimation(PlayerTriggerAnimation animationToPlay)
        {
            playerAnimator.SetTrigger(animationToPlay.ToString());
        }
    }
}