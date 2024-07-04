using System.Collections;
using Platformer.AnimationSystem;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Player
{
    public class PlayerController
    {
        // References:
        private PlayerScriptableObject playerScriptableObject;
        public PlayerView PlayerView { get; private set; }
        public PlayerAnimationController PlayerAnimationController { get; private set; }
        private Animator animator;
        
        // Variables:
        private PlayerState currentPlayerState;
        private int currentHealth;
        private float currentSpeed;
        private float cachedHorizontalInput;
        
        // Properties:
        public int CurrentHealth {
            get => currentHealth;
            private set => currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
        }
        
        private bool IsGrounded()
        {
            var offset = 0.3f;
            var raycastHit = Physics2D.Raycast(PlayerView.GetColliderBounds().center, Vector2.down, PlayerView.GetColliderBounds().extents.y + offset, PlayerView.GroundLayer);
            return raycastHit.collider != null;
        }

        public PlayerController(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            InitializeView();
            InitializeVariables();
            InitializePlayerAnimationController();
            SubscribeToEvents();
        }

        private void InitializePlayerAnimationController()
        {
            PlayerAnimationController = new PlayerAnimationController();
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
        
        private void SubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived += HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived += HandleTriggerInput;
        }

        private void UnsubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived -= HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived -= HandleTriggerInput;
        }
      
        // TODO: The listener method should simply cache the horizontalInput for the Player.
        // TODO: The movement should happen through update accordingly, else there will be an independent flow for movement which may not consider other state chanegs for the player.
        public void HandleHorizontalMovementAxisInput(float horizontalInput)
        {
            cachedHorizontalInput = horizontalInput;
        }
        
        private void MovePlayer(float cachedHorizontalInput)
        {
            UpdateRunningStatus();
            SetPlayerSpriteDirection();
            TranslatePlayer();
            
            PlayerAnimationController.PlayMovementAnimation(currentPlayerState);
            
        }
        
        private void UpdateRunningStatus()
        {
            SetPlayerState(cachedHorizontalInput != 0 ? PlayerState.RUNNING : PlayerState.IDLE);
        }
        
        private bool CanRun() => currentPlayerState != PlayerState.SLIDE;

        // TODO: This check is not needed inside PlayerController.
        // TODO: Here we are essentially checking if any horizontal input is received or not.
        // TODO: This should be checked inside Input Service and only if the input is received the event should be fired!
        
        private void SetPlayerSpriteDirection()
        {
            if (cachedHorizontalInput != 0)
                PlayerView.SetCharacterSpriteDirection(cachedHorizontalInput < 0);
        }
        
        private void TranslatePlayer()
        {
            var movementVector = new Vector3(cachedHorizontalInput, 0.0f, 0.0f).normalized;
            PlayerView.TranslatePlayer(currentSpeed * Time.deltaTime * movementVector);
        }

        // TODO: This Update should be called by ServiceLocator -> PlayerService; not from the PlayerView
        // TODO: To have control over the order of execution and game loop. 
        public void Update()
        {
            HandleJumpAndFall();
            MovePlayer(cachedHorizontalInput);
        }

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
                    PlayerAnimationController.PlayDamageAnimation();
                    break;
            }
        }

        private void ProcessJumpInput()
        {
            if(CanJump()){
                Jump();
                PlayerAnimationController.PlayJumpAnimation();
            }
        }

        private bool CanJump() => IsGrounded() && (currentPlayerState == PlayerState.IDLE || currentPlayerState == PlayerState.RUNNING);

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
                currentPlayerState = PlayerState.ATTACK;
                PlayerAnimationController.PlayAttackAnimation();
            }
        }

        private bool CanAttack() => IsGrounded() && (currentPlayerState == PlayerState.IDLE || currentPlayerState == PlayerState.RUNNING);

        private void ProcessSlideInput()
        {
            if (CanSlide())
            {
                // TODO: Where is this coroutine?
                //CoroutineService.StartCoroutine(SlideCoroutine(playerScriptableObject.slidingTime), "PlayerSlide");
                PlayerAnimationController.PlaySlideAnimation();
            }
        }
        
        private bool CanSlide() => IsGrounded() && currentPlayerState == PlayerState.RUNNING;

        //redundant method:
        // private void FlipSpriteIfNeeded(float horizontalInput)
        // {
        //     SetPlayerState(PlayerState.SLIDE);
        //     //yield return new WaitForSeconds(slidingTime);
        //     SetPlayerState(PlayerState.IDLE);
        // }
        
        

        public void TakeDamage(int damageToInflict)
        {
            CurrentHealth -= damageToInflict;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                PlayerDied();
            }else{
                PlayerAnimationController.PlayDamageAnimation();
            }
        }

        private void PlayerDied()
        {
            UnsubscribeToEvents();
            PlayerAnimationController.PlayDeathAnimation();
            CoroutineService.StartCoroutine(DelayedRespawnCoroutine(), "PlayerRespawn");
        }
        
        private IEnumerator DelayedRespawnCoroutine()
        {
            yield return new WaitForSeconds(playerScriptableObject.delayAfterDeath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public float GetFallMultiplier() => playerScriptableObject.fallMultiplier;

        public float GetLowJumpMultiplier() => playerScriptableObject.lowJumpMultiplier;

        private void SetPlayerState(PlayerState newState)
        {
            currentPlayerState = newState;
            UpdateCurrentSpeed();
        }

        private void UpdateCurrentSpeed()
        {
            switch (currentPlayerState)
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