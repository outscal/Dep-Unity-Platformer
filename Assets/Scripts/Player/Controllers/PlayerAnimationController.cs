using Platformer.AnimationSystem;
using Platformer.Main;
using Platformer.Player.Enumerations;
using UnityEngine;

namespace Platformer.Player.Controllers
{
    public class PlayerAnimationController
    {
        private Animator playerAnimator;
        private AnimationService animationService;

        PlayerAnimationController(Animator playerAnimator)
        {
            this.playerAnimator = playerAnimator;
            animationService = GameService.Instance.AnimationService;
        } 
        
        public void PlayMovementAnimation(PlayerState currentPlayerState)
        {
            animationService.ToggleBoolAnimation(playerAnimator, PlayerState.RUNNING.ToString(), currentPlayerState == PlayerState.RUNNING);
        }
        
        public void PlayTriggerAnimation(PlayerTriggerAnimationType triggerType)
        {
            animationService.PlayTriggerAnimation(playerAnimator, triggerType.ToString());
        }
    }
}