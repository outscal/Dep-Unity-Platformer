using Platformer.InputSystem;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController
    {
        #region Service References
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerView PlayerView { get; private set; }

        #region Health
        private int currentHealth;
        public int CurrentHealth {
            get => currentHealth;
            private set => currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
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

        private void InitializeVariables() => CurrentHealth = playerScriptableObject.maxHealth;

        #region Handle Player Input

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
            // every case can have custom logic for movement or something else
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
                case PlayerInputTriggers.TAKE_DAMAGE: // temporary switch case (just for testing animation)
                    PlayerService.PlayDamageAnimation(PlayerView.PlayerAnimator);
                    break;
            }
        }

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
                PlayerService.PlayDamageAnimation(PlayerView.PlayerAnimator);
            }
        }

        private void PlayerDied() => PlayerService.PlayerDied(PlayerView.PlayerAnimator);

        public float GetGravityDownForce() => playerScriptableObject.gravityDownForceMultiplier;

        public float GetFallMultiplier() => playerScriptableObject.fallMultiplier;

        public float GetLowJumpMultiplier() => playerScriptableObject.lowJumpMultiplier;
    }
}