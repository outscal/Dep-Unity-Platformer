using UnityEngine;
using Platformer.Player.Enumerations;

namespace Platformer.AnimationSystem
{
    public class AnimationService
    {
        public void PlayPlayerMovementAnimation(Animator playerAnimator, PlayerState currentPlayerState)
        {
            playerAnimator.SetBool(PlayerAnimationType.RUNNING.ToString(), currentPlayerState == PlayerState.RUNNING);
        }

        public void PlayPlayerTriggerAnimation(Animator playerAnimator, PlayerTriggerAnimationType animationToPlay)
        {
            playerAnimator.SetTrigger(animationToPlay.ToString());
        }
    }
}