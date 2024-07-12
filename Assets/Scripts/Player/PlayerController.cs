using System;
using System.Threading.Tasks;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Game;
using Platformer.UI;
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
        private PlayerService PlayerService => GameService.Instance.PlayerService;
        private UIService UIService => GameService.Instance.UIService;
        
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
        
        //Action-Events
        public static event Action<GameEndType> OnGameEnd;

        public int CurrentHealth
        {
            get => currentHealth;
            set {
                currentHealth = Mathf.Clamp(value, 0, playerScriptableObject.maxHealth);
                UIService.UpdatePlayerHealthUI((float)currentHealth / playerScriptableObject.maxHealth);
            }
        }

        //Coins/Score
        private int currentCoins = 0;
        public int CurrentCoins { get => currentCoins; 
            private set{
                currentCoins = value;
                UIService.UpdateCoinsCountUI(currentCoins);
            }
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
            CurrentCoins = 0;
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

        public void PlayerDied()
        {
            UnsubscribeToEvents();
            animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.DEATH);
            coroutineIdRespawn = CoroutineService.StartCoroutine(GameEndCoroutine());
        }
        
        private IEnumerator GameEndCoroutine()
        {
            yield return new WaitForSeconds(playerScriptableObject.delayAfterDeath);
            OnGameEnd?.Invoke(GameEndType.LOSE);
            CleanEventListeners();
        }
        
        public void SetPlayerState(PlayerState newState)
        {
            currentPlayerState = newState;
            movementController?.UpdateCurrentSpeed();
        }

        public PlayerState GetPlayerState() => currentPlayerState;
        
        public void TakeDamage(int damage)
        {
            bool isDead = combatController.TakeDamage(damage);
            if(!isDead)
                animationController.PlayTriggerAnimation(PlayerTriggerAnimationType.TAKE_DAMAGE);
            
        }

        private void CleanEventListeners() => OnGameEnd = null;
        
        ~PlayerController()
        {
            playerView.DeletePlayer();
            playerView = null;
        }
    }
}