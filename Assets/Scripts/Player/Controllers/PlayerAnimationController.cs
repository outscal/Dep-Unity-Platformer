using Platformer.AnimationSystem;
using Platformer.Main;
using Platformer.Player.Enumerations;
using UnityEngine;

namespace Platformer.Player.Controllers
{
    public class PlayerAnimationController
    {
        private PlayerController owner;
        private PlayerView playerView;
        private AnimationService animationService;

        public PlayerAnimationController(PlayerController owner, PlayerView playerView)
        {
            this.owner = owner;
            this.playerView = playerView;
            animationService = GameService.Instance.AnimationService;
        }

        public void Update(float currentHorizontalInput)
        {
            SetPlayerSpriteDirection(currentHorizontalInput);
            PlayMovementAnimation(owner.GetPlayerState());
        }
        
        private void SetPlayerSpriteDirection(float currentHorizontalInput)
        {
            if (currentHorizontalInput != 0)
                playerView.SetCharacterSpriteDirection(currentHorizontalInput < 0);
        }
        
        public void PlayMovementAnimation(PlayerState currentPlayerState)
        {
            animationService.ToggleBoolAnimation(playerView.PlayerAnimator, PlayerState.RUNNING.ToString(), currentPlayerState == PlayerState.RUNNING);
        }
        
        public void PlayTriggerAnimation(PlayerTriggerAnimationType triggerType)
        {
            animationService.PlayTriggerAnimation(playerView.PlayerAnimator, triggerType.ToString());
        }
    }
}