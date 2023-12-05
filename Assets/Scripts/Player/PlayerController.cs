using Platformer.InputSystem;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController
    {
        #region Service References
        private PlayerService playerService => GameService.Instance.PlayerService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        private PlayerView playerView;

        public PlayerController(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            InitializeView();
        }

        private void InitializeView(){
            playerView = Object.Instantiate(playerScriptableObject.prefab);
            playerView.transform.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            playerView.SetController(this);
        }

        public void HandleHorizontalMovementAxisInput(float horizontalInput){
            var movementDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
            if(movementDirection != Vector3.zero)
            {
                // physically move the player
                playerView.Move(horizontalInput);
                playerService.PlayMovementAnimation(playerView.PlayerAnimator, true);
            }else{
                playerService.PlayMovementAnimation(playerView.PlayerAnimator, false);
            }
        }

        public void HandleTriggerInput(PlayerInputTriggers playerInputTriggers){
            // every case can have custom logic for movement or something else
            switch (playerInputTriggers)
            {
                case PlayerInputTriggers.JUMP:
                    playerService.PlayJumpAnimation(playerView.PlayerAnimator);
                    break;
                case PlayerInputTriggers.ATTACK:
                    playerService.PlayAttackAnimation(playerView.PlayerAnimator);
                    break;
                case PlayerInputTriggers.DEATH:
                    playerService.PlayDeathAnimation(playerView.PlayerAnimator);
                    break;
                case PlayerInputTriggers.SLIDE:
                    playerService.PlaySlideAnimation(playerView.PlayerAnimator);
                    break;
                case PlayerInputTriggers.TAKE_DAMAGE:
                    playerService.PlayDamageAnimation(playerView.PlayerAnimator);
                    break;
            }
        }

        public float GetPlayerMovementSpeed() => playerScriptableObject.movementSpeed;
    }
}