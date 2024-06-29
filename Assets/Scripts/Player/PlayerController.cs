using System.Collections;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Services;
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

        private PlayerStates playerState;
        private float playerTranslateSpeed;

        #region Health
        private int currentHealth;
        public int CurrentHealth {
            get => currentHealth;
            private set => currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
        }
        #endregion

        #region Grounded
        private bool IsGrounded {
            get {
                var offset = 0.3f;
                var raycastHit = Physics2D.Raycast(PlayerView.GetColliderBounds().center, Vector2.down, PlayerView.GetColliderBounds().extents.y + offset, PlayerView.GroundLayer);
                return raycastHit.collider != null;
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
            PlayerView.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            PlayerView.SetController(this);
        }

        private void InitializeVariables()
        {
            CurrentHealth = playerScriptableObject.maxHealth;
            SetPlayerState(PlayerStates.IDLE);
        }

        public void Update() => HandleJumpAndFall();
      
        #region Handle Player Input
        public void HandleHorizontalMovementAxisInput(float horizontalInput) => MovePlayer(horizontalInput);
        public void HandleTriggerInput(PlayerInputTriggers playerInputTriggers)
        {
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
        #endregion

        #region Player movement

        private void MovePlayer(float horizontalInput)
        {
            UpdateRunningStatus(horizontalInput);
            SetPlayerSpriteDirection(horizontalInput);
            TranslatePlayer(horizontalInput);
            PlayerService.MovePlayer(PlayerView.PlayerAnimator, IsRunning(horizontalInput), PlayerView.Position);
        }

        private bool IsRunning(float horizontalInput) => horizontalInput != 0;
        private void UpdateRunningStatus(float horizontalInput)
        {
            if (playerState == PlayerStates.SLIDE)
                return;
            else if (IsRunning(horizontalInput))
                SetPlayerState(PlayerStates.RUNNING);
            else
                SetPlayerState(PlayerStates.IDLE);
        }
        private void SetPlayerSpriteDirection(float horizontalInput)
        {
            if (horizontalInput != 0)
                PlayerView.SetCharacterSpriteDirection(horizontalInput < 0);
        }
        private void TranslatePlayer(float horizontalInput)
        {
            var movementVector = new Vector3(horizontalInput, 0.0f, 0.0f).normalized;
            PlayerView.TranslatePlayer(playerTranslateSpeed * Time.deltaTime * movementVector);
        }

        #endregion

        #region Player Jump and Fall
        private void HandleJumpAndFall()
        {
            if (getIsFalling())
            {
                //Increase Falling Velocity
                PlayerView.AddVelocity((GetFallMultiplier()) * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
            else if (getIsJumping())
            {
                //Decrease Jumping Velocity
                PlayerView.AddVelocity((GetLowJumpMultiplier()) * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
        }

        private bool getIsFalling() => PlayerView.GetVelocity().y < 0;
        private bool getIsJumping() => PlayerView.GetVelocity().y > 0;


        private void ProcessJumpInput()
        {
            if(CanJump()){
                Jump();
                PlayerService.PlayJumpAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanJump() => IsGrounded && (playerState == PlayerStates.IDLE || playerState == PlayerStates.RUNNING);

        // JUMP 1st IMPLEMENTATION
        public void Jump() => PlayerView.SetVelocity(Vector2.up * playerScriptableObject.jumpForce);

        // JUMP 2nd IMPLEMENTATION
        // public void Jump() => PlayerView.AddForce(Vector2.up * playerScriptableObject.jumpForce, ForceMode2D.Impulse);

        // JUMP 3rd IMPLEMENTATION
        /*private void Jump() // direct changing the position through translate method
        {
            var force = playerScriptableObject.jumpForce;
            float jumpHeight = force * Time.deltaTime; // Convert force to a height
            PlayerView.TranslatePlayer(Vector2.up * jumpHeight);
        }*/

        #endregion

        #region Attack
        private void ProcessAttackInput(){
            if(CanAttack()){
                playerState = PlayerStates.ATTACK;
                PlayerService.PlayAttackAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanAttack() => IsGrounded && (playerState == PlayerStates.IDLE || playerState == PlayerStates.RUNNING);
        #endregion

        #region Slide

        private bool CanSlide() => IsGrounded && playerState == PlayerStates.RUNNING;

        private void ProcessSlideInput()
        {
            if (CanSlide())
            {
                CoroutineService.StartCoroutine(SlideCoroutine(playerScriptableObject.slidingTime), "PlayerSlide");
                PlayerService.PlaySlideAnimation(PlayerView.PlayerAnimator);
            }
        }

        private IEnumerator SlideCoroutine(float slidingTime)
        {
            SetPlayerState(PlayerStates.SLIDE);
            yield return new WaitForSeconds(slidingTime);
            SetPlayerState(PlayerStates.IDLE);
        }

        #endregion

        #region Damage/Death
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
        #endregion

        #region Getter Functions
        public float GetFallMultiplier() => playerScriptableObject.fallMultiplier;

        public float GetLowJumpMultiplier() => playerScriptableObject.lowJumpMultiplier;
        #endregion

        private void SetPlayerState(PlayerStates new_state)
        {
            playerState = new_state;
            UpdatePlayerStats();
        }

        private void UpdatePlayerStats()
        {
            switch (playerState)
            {
                case PlayerStates.IDLE:
                    playerTranslateSpeed = playerScriptableObject.movementSpeed;
                    break;
                case PlayerStates.RUNNING: 
                    playerTranslateSpeed = playerScriptableObject.movementSpeed;
                    break;
                case PlayerStates.SLIDE:
                    playerTranslateSpeed = playerScriptableObject.slidingSpeed;
                    break;
                case PlayerStates.ATTACK: 
                    playerTranslateSpeed = playerScriptableObject.movementSpeed;
                    break;
            }
        }
    }
}