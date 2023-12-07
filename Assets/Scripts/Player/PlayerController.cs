using Platformer.InputSystem;
using Platformer.Main;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController
    {
        #region Service References
        private PlayerService playerService => GameService.Instance.PlayerService;
        private UIService UIService => GameService.Instance.UIService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerView playerView { get; private set; }

        #region Health
        private int currentHealth;
        public int CurrentHealth {
            get => currentHealth;
            private set {
                currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
                UIService.UpdatePlayerHealth((float)currentHealth / playerScriptableObject.maxHealth);
            }
        }
        #endregion

        #region coins/Score
        private int currentCoins = 0;
        public int CurrentCoins { get => currentCoins; 
            private set{
                currentCoins = value;
                UIService.UpdateCoinsCount(currentCoins);
            }
        }
        #endregion

        public PlayerController(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            InitializeView();
            InitializeVariables();
        }

        private void InitializeView(){
            playerView = Object.Instantiate(playerScriptableObject.prefab);
            playerView.transform.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            playerView.SetController(this);
        }

        private void InitializeVariables()
        {
            CurrentCoins = 0;
            CurrentHealth = playerScriptableObject.maxHealth;
        }

        public void HandleHorizontalMovementAxisInput(float horizontalInput){
            var movementDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
            if(movementDirection != Vector3.zero)
            {
                playerView.Move(horizontalInput, playerScriptableObject.movementSpeed);
                playerService.MovePlayer(playerView.PlayerAnimator, true, playerView.Position);
            }else{
                playerService.MovePlayer(playerView.PlayerAnimator, false, playerView.Position);
            }
        }

        public void HandleTriggerInput(PlayerInputTriggers playerInputTriggers){
            switch (playerInputTriggers)
            {
                case PlayerInputTriggers.JUMP:
                    if(playerView.CanJump()){
                        playerView.Jump(playerScriptableObject.jumpForce);
                        playerService.PlayJumpAnimation(playerView.PlayerAnimator);
                    }
                    break;
                case PlayerInputTriggers.ATTACK:
                    playerService.PlayAttackAnimation(playerView.PlayerAnimator);
                    break;
                case PlayerInputTriggers.SLIDE:
                    if(playerView.CanSlide()){
                        playerView.Slide(playerScriptableObject.slidingSpeed, playerScriptableObject.slidingTime);
                        playerService.PlaySlideAnimation(playerView.PlayerAnimator);
                    }
                    break;
            }
        }

        public void TakeDamage(int damageToInflict)
        {
            CurrentHealth -= damageToInflict;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                PlayerDied();
            }else{
                playerService.PlayTakeDamageAnimation(playerView.PlayerAnimator);
            }
        }

        private void PlayerDied() => playerService.PlayerDied(playerView.PlayerAnimator);
    }
}