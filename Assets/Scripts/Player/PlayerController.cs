using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Platformer.InputSystem;
using Platformer.Player.Controllers;
using Platformer.Player.Enumerations;
using Platformer.Services;
using Platformer.Utilities;
using Object = UnityEngine.Object;

namespace Platformer.Player
{
    public class PlayerController : IDamagable
    {
        // References:
        private PlayerView playerView;
        private PlayerScriptableObject playerScriptableObject;
        
        // Sub-Controllers:
        private PlayerMovementController movementController;
        private PlayerAnimationController animationController;
        private PlayerCombatController combatController;
        
        // Variables:
        private PlayerState currentPlayerState;
        private int currentHealth;
        private float cachedHorizontalInput;
        private int coroutineIdRespawn;

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
        }
        
        public PlayerController(PlayerScriptableObject playerScriptableObject)
        {
            InitializeVariables(playerScriptableObject);
            InitializeView();
            InitializeControllers();
            SubscribeToEvents();
        }

        private void InitializeVariables(PlayerScriptableObject playerScriptableObject)
        {
            this.playerScriptableObject = playerScriptableObject;
            CurrentHealth = playerScriptableObject.maxHealth;
            SetPlayerState(PlayerState.IDLE);
        }

        private void InitializeView()
        {
            playerView = Object.Instantiate(playerScriptableObject.prefab);
            playerView.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            playerView.SetController(this);
        }

        private void InitializeControllers()
        {
            animationController = new PlayerAnimationController(this, playerView);
            movementController = new PlayerMovementController(this, playerView, playerScriptableObject);
            combatController = new PlayerCombatController(this);
        }
        
        private void SubscribeToEvents()
        {
            InputService.OnHorizontalAxisInputReceived += UpdateHorizontalAxisInput;
            InputService.OnPlayerTriggerInputReceived += HandleTriggerInput;
        }

        private void UnsubscribeToEvents()
        {
            InputService.OnHorizontalAxisInputReceived -= UpdateHorizontalAxisInput;
            InputService.OnPlayerTriggerInputReceived -= HandleTriggerInput;
        }

        public void Update()
        {
            movementController.Update(cachedHorizontalInput);
            animationController.Update(cachedHorizontalInput);
        }
        
        public void UpdateHorizontalAxisInput(float horizontalInput) => cachedHorizontalInput = horizontalInput;
        
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
                    ProcessTakeDamageInput();
                    break;
            }
        }

        private void ProcessJumpInput()
        {
            if(movementController.CanJump())
            {
                movementController.Jump();
                animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.JUMP);
            }
        }

        private void ProcessAttackInput()
        {
            if(combatController.CanAttack(movementController.IsGrounded(), currentPlayerState))
            {
                SetPlayerState(PlayerState.ATTACK);
                animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.ATTACK);
            }
        }

        private void ProcessSlideInput()
        {
            if (movementController.CanSlide())
            {
                movementController.Slide(cachedHorizontalInput);
                animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.SLIDE);
            }
        }

        private void ProcessTakeDamageInput()
        {
            animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.TAKE_DAMAGE);
        }

        public void PlayerDied()
        {
            UnsubscribeToEvents();
            animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.DEATH);
            coroutineIdRespawn = CoroutineService.StartCoroutine(DelayedRespawnCoroutine());
        }
        
        private IEnumerator DelayedRespawnCoroutine()
        {
            yield return new WaitForSeconds(playerScriptableObject.delayAfterDeath);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        
        public void SetPlayerState(PlayerState newState)
        {
            currentPlayerState = newState;
            movementController.UpdateCurrentSpeed();
        }

        public PlayerState GetPlayerState() => currentPlayerState;
        
        public void TakeDamage(int damage)
        {
            bool isDead = combatController.TakeDamage(damage);
            if(!isDead)
                animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.DEATH);
            
        }  
    }
}