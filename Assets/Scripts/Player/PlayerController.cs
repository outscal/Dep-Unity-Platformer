using System.Collections;
using Platformer.AnimationSystem;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Services;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController
    {
        // Dependencies:
        // TODO: This dependency on Player Service should be removed.
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        private AnimationService animationService => GameService.Instance.AnimationService;

        // References:
        private PlayerScriptableObject playerScriptableObject;
        public PlayerView PlayerView { get; private set; }
        private Animator animator;
        
        // Variables:
        private PlayerState currentState;
        private int currentHealth;
        private float currentSpeed;
        
        // Properties:
        public int CurrentHealth {
            get => currentHealth;
            private set => currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
        }

        // TODO: This should be implemented as a proper method instead as a Property like below.
        // TODO: Magic Variables are being used here, add this offset data in PlayerSO if needed.
        private bool IsGrounded {
            get {
                var offset = 0.3f;
                var raycastHit = Physics2D.Raycast(PlayerView.GetColliderBounds().center, Vector2.down, PlayerView.GetColliderBounds().extents.y + offset, PlayerView.GroundLayer);
                return raycastHit.collider != null;
            }
        }

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
            SetPlayerState(PlayerState.IDLE);
        }
      
        // TODO: The listener method should simply cache the horizontalInput for the Player.
        // TODO: The movement should happen through update accordingly, else there will be an independent flow for movement which may not consider other state chanegs for the player.
        public void HandleHorizontalMovementAxisInput(float horizontalInput) => MovePlayer(horizontalInput);
        
        private void MovePlayer(float horizontalInput)
        {
            // TODO: 
            // if(CanRun())
            //    UpdateRunningState();
            
            UpdateRunningStatus(horizontalInput);
            SetPlayerSpriteDirection(horizontalInput);
            TranslatePlayer(horizontalInput);
            
            // TODO: Its not PlayerService's responsibility to play animations for the Player.
            PlayerService.MovePlayer(PlayerView.PlayerAnimator, IsRunning(horizontalInput), PlayerView.Position);
        }
        
        private void UpdateRunningStatus(float horizontalInput)
        {
            // TODO: Check should be outside the function. Better if you create a CanRun() method; in case more checks can be added in that function in future.
            if (currentState == PlayerState.SLIDE)
                return;
            
            if (IsRunning(horizontalInput))
                SetPlayerState(PlayerState.RUNNING);
            else
                SetPlayerState(PlayerState.IDLE);
        }

        // TODO: This check is not needed inside PlayerController.
        // TODO: Here we are essentially checking if any horizontal input is received or not.
        // TODO: This should be checked inside Input Service and only if the input is received the event should be fired!
        private bool IsRunning(float horizontalInput) => horizontalInput != 0;
        
        private void SetPlayerSpriteDirection(float horizontalInput)
        {
            if (horizontalInput != 0)
                PlayerView.SetCharacterSpriteDirection(horizontalInput < 0);
        }
        
        private void TranslatePlayer(float horizontalInput)
        {
            var movementVector = new Vector3(horizontalInput, 0.0f, 0.0f).normalized;
            PlayerView.TranslatePlayer(currentSpeed * Time.deltaTime * movementVector);
        }

        // TODO: This Update should be called by ServiceLocator -> PlayerService; not from the PlayerView
        // TODO: To have control over the order of execution and game loop. 
        public void Update() => HandleJumpAndFall();
        
        private void HandleJumpAndFall()
        {
            if (isFalling())
            {
                //Increase Falling Velocity
                PlayerView.AddVelocity((GetFallMultiplier()) * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
            else if (isJumping())
            {
                //Decrease Jumping Velocity
                PlayerView.AddVelocity((GetLowJumpMultiplier()) * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
        }

        private bool isFalling() => PlayerView.GetVelocity().y < 0;
        
        private bool isJumping() => PlayerView.GetVelocity().y > 0;
        
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

        private void ProcessJumpInput()
        {
            if(CanJump()){
                Jump();
                PlayerService.PlayJumpAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanJump() => IsGrounded && (currentState == PlayerState.IDLE || currentState == PlayerState.RUNNING);

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


        private void ProcessAttackInput(){
            if(CanAttack()){
                currentState = PlayerState.ATTACK;
                PlayerService.PlayAttackAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanAttack() => IsGrounded && (currentState == PlayerState.IDLE || currentState == PlayerState.RUNNING);

        private void ProcessSlideInput()
        {
            if (CanSlide())
            {
                // TODO: Where is this coroutine?
                CoroutineService.StartCoroutine(SlideCoroutine(playerScriptableObject.slidingTime), "PlayerSlide");
                PlayerService.PlaySlideAnimation(PlayerView.PlayerAnimator);
            }
        }
        
        private bool CanSlide() => IsGrounded && currentState == PlayerState.RUNNING;

        // TODO: Is this a redundant method?
        private void FlipSpriteIfNeeded(float horizontalInput)
        {
            SetPlayerState(PlayerStates.SLIDE);
            yield return new WaitForSeconds(slidingTime);
            SetPlayerState(PlayerStates.IDLE);
        }
        
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

        public float GetFallMultiplier() => playerScriptableObject.fallMultiplier;

        public float GetLowJumpMultiplier() => playerScriptableObject.lowJumpMultiplier;

        private void SetPlayerState(PlayerState newState)
        {
            currentState = newState;
            UpdateCurrentSpeed();
        }

        private void UpdateCurrentSpeed()
        {
            switch (currentState)
            {
                case PlayerState.IDLE:
                    currentSpeed = 0f;
                    break;
                case PlayerState.RUNNING: 
                    currentSpeed = playerScriptableObject.movementSpeed;
                    break;
                case PlayerState.SLIDE:
                    currentSpeed = playerScriptableObject.slidingSpeed;
                    break;
                case PlayerState.ATTACK: 
                    currentSpeed = 0f;
                    break;
            }
        }
    }
}