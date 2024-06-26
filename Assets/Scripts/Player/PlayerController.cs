using Platformer.AnimationSystem;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private AnimationService animation_service;

        private void Awake()
        {
            animation_service = new AnimationService(animator);
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            HandleMovementInput();
            HandleTriggerInput();
        }

        private void HandleMovementInput()
        {
            // Check for movement
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            bool isRunning = Mathf.Abs(horizontalInput) > 0.1f;
            PlayPlayerMovementAnimation(isRunning);

            if (isRunning)
            {
                FlipSpriteIfNeeded(horizontalInput);
            }
        }

        private void FlipSpriteIfNeeded(float horizontalInput)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), transform.localScale.y, transform.localScale.z);
        }

        private void PlayPlayerMovementAnimation(bool isRunning)
        {
            animation_service.PlayPlayerMovementAnimation(isRunning);
        }

        private void HandleTriggerInput()
        {

            if (Input.GetKeyDown(KeyCode.Space))
                animation_service.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.JUMP);
            if (Input.GetKeyDown(KeyCode.C))
                animation_service.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.SLIDE);
            if (Input.GetKeyDown(KeyCode.X))
                animation_service.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.ATTACK);
            if (Input.GetKeyDown(KeyCode.J))
                animation_service.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.TAKE_DAMAGE);
            if (Input.GetKeyDown(KeyCode.K))
                animation_service.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.DEATH);
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