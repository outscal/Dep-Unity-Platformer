using System.Collections.Generic;
using Platformer.Enemy;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController
    {
        #region Service References
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        private UIService UIService => GameService.Instance.UIService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerView PlayerView { get; private set; }

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
            PlayerView = Object.Instantiate(playerScriptableObject.prefab);
            PlayerView.transform.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            PlayerView.SetController(this);
        }

        private void InitializeVariables()
        {
            CurrentCoins = 0;
            CurrentHealth = playerScriptableObject.maxHealth;
        }

        #region Player Input Handling

        public void HandleHorizontalMovementAxisInput(float horizontalInput){
            var movementDirection = new Vector3(horizontalInput, 0f, 0f).normalized;
            if(movementDirection != Vector3.zero)
            {
                // physically move the player
                PlayerView.Move(horizontalInput, playerScriptableObject.movementSpeed);
                PlayerService.MovePlayer(PlayerView.PlayerAnimator, true, PlayerView.Position);
            }else{
                PlayerService.MovePlayer(PlayerView.PlayerAnimator, false, PlayerView.Position);
            }
        }

        public void HandleTriggerInput(PlayerInputTriggers playerInputTriggers){
            switch (playerInputTriggers)
            {
                case PlayerInputTriggers.JUMP:
                    ProcessJumpInput();
                    break;
                case PlayerInputTriggers.ATTACK:
                    ProcessAttackInput();
                    break;
                case PlayerInputTriggers.SLIDE:
                    ProcessSlideInput();
                    break;
            }
        }

        private void Attack() => _ = new MeleeController(playerScriptableObject.meleeSO, playerView.MeleeContainer);
        private void ProcessJumpInput(){
            if(PlayerView.CanJump()){
                PlayerView.Jump(playerScriptableObject.jumpForce);
                PlayerService.PlayJumpAnimation(PlayerView.PlayerAnimator);
            }
        }

        private void ProcessAttackInput(){
            if(PlayerView.CanAttack()){
                PlayerView.Attack();
                PlayerService.PlayAttackAnimation(PlayerView.PlayerAnimator);
            }
        }

        private void ProcessSlideInput(){
            if(PlayerView.CanSlide()){
                PlayerView.Slide(playerScriptableObject.slidingSpeed, playerScriptableObject.slidingTime);
                PlayerService.PlaySlideAnimation(PlayerView.PlayerAnimator);
            }
        }

        #endregion

        public void Die() => PlayerService.PlayDeathAnimation(PlayerView.PlayerAnimator);

        public void TakeDamage(int damageToInflict)
        {
            CurrentHealth -= damageToInflict;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                PlayerDied();
            }else{
                PlayerService.TakeDamage(PlayerView.PlayerAnimator);
            }
        }

        private void PlayerDied() => PlayerService.PlayerDied(PlayerView.PlayerAnimator);

        public float GetGravityDownForce() => playerScriptableObject.gravityDownForceMultiplier;

        public float GetFallMultiplier() => playerScriptableObject.fallMultiplier;

        public float GetLowJumpMultiplier() => playerScriptableObject.lowJumpMultiplier;
    }
}