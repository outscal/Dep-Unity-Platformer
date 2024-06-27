using System.Threading.Tasks;
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
                var raycastHit = Physics2D.Raycast(PlayerView.PlayerBoxCollider.bounds.center, Vector2.down, PlayerView.PlayerBoxCollider.bounds.extents.y + offset, PlayerView.GroundLayer);
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
            PlayerView.transform.SetPositionAndRotation(playerScriptableObject.spawnPosition, Quaternion.Euler(playerScriptableObject.spawnRotation));
            PlayerView.SetController(this);
        }

        private void InitializeVariables() => CurrentHealth = playerScriptableObject.maxHealth;

        public void Update(){
            var check = IsGrounded;
            if(PlayerView.PlayerRigidBody.velocity.y < 0){
                PlayerView.PlayerRigidBody.velocity += (GetFallMultiplier() - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }else if(PlayerView.PlayerRigidBody.velocity.y > 0){
                PlayerView.PlayerRigidBody.velocity += (GetLowJumpMultiplier() - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }
        }


        #region Handle Player Input

        #region Player movement
        public void HandleHorizontalMovementAxisInput(float horizontalInput) => MovePlayer(horizontalInput);

        private void MovePlayer(float horizontalInput)
        {
            UpdateRunningStatus(horizontalInput);
            SetPlayerSpriteDirection(horizontalInput);
            SetMovementSpeed();
            TranslatePlayer(horizontalInput);
            PlayerService.MovePlayer(PlayerView.PlayerAnimator, getIsRunning(horizontalInput), PlayerView.Position);
        }

        private bool getIsRunning(float horizontalInput) => horizontalInput != 0;
        private void UpdateRunningStatus(float horizontalInput) => playerState = getIsRunning(horizontalInput) ? PlayerStates.RUNNING : PlayerStates.IDLE;
        private void SetMovementSpeed()
        {
            switch(playerState)
            {
                case PlayerStates.RUNNING:
                    playerTranslateSpeed = playerScriptableObject.movementSpeed;
                    break;
                case PlayerStates.SLIDE:
                    playerTranslateSpeed = playerScriptableObject.slidingSpeed;
                    break;
                default:
                    playerTranslateSpeed = playerScriptableObject.movementSpeed;
                    break;
            }
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

        #region trigger input
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
            if(CanJump()){
                PlayerView.Jump(playerScriptableObject.jumpForce);
                PlayerService.PlayJumpAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanJump() => IsGrounded && (playerState == PlayerStates.IDLE || playerState == PlayerStates.RUNNING);

        private void ProcessAttackInput(){
            if(CanAttack()){
                playerState = PlayerStates.ATTACK;
                PlayerService.PlayAttackAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanAttack() => IsGrounded && (playerState == PlayerStates.IDLE || playerState == PlayerStates.RUNNING);

        private void ProcessSlideInput(){
            if(CanSlide()){
                Slide(playerScriptableObject.slidingSpeed, playerScriptableObject.slidingTime);
                PlayerService.PlaySlideAnimation(PlayerView.PlayerAnimator);
            }
        }

        private bool CanSlide() => IsGrounded && playerState == PlayerStates.RUNNING;

        private async void Slide(float slidingSpeed, float slidingTime){
            var temp = playerTranslateSpeed;
            SetSlidingState(slidingSpeed, true);
            await Task.Delay((int)(slidingTime * 1000));
            SetSlidingState(temp, false);
        }

        private void SetSlidingState(float speed, bool isSliding)
        {
            playerTranslateSpeed = speed;
            playerState = isSliding ? PlayerStates.SLIDE : PlayerStates.IDLE;
        }
        #endregion
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
    }
}