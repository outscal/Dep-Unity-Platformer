using Platformer.AnimationSystem;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private AnimationService animationService;
        private PlayerState currentState;

        private void Awake()
        {
            if(animator != null)
                animationService = new AnimationService(animator);
            currentState = PlayerState.IDLE;
        }

        private void Update() => HandleInput();

        private void HandleInput()
        {
            HandleMovementInput();
            HandleTriggerInput();
        }

        private void HandleMovementInput()
        {
            // Check for movement input
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            
            // Set Current Player State
            currentState = (Mathf.Abs(horizontalInput) > 0.1f) ? PlayerState.RUNNING : PlayerState.IDLE;

            // PLay Movement Animations
            PlayMovementAnimation();
            
            // Flip Player Sprite if needed
            FlipSpriteIfNeeded(horizontalInput);
        }

        private void FlipSpriteIfNeeded(float horizontalInput)
        {
            if (currentState == PlayerState.RUNNING)
                transform.localScale = new Vector3(Mathf.Sign(horizontalInput), transform.localScale.y, transform.localScale.z);
        }

        private void PlayMovementAnimation() => animationService.PlayPlayerMovementAnimation(currentState);

        private void HandleTriggerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                animationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimationType.JUMP);
            if (Input.GetKeyDown(KeyCode.C))
                animationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimationType.SLIDE);
            if (Input.GetKeyDown(KeyCode.X))
                animationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimationType.ATTACK);
            if (Input.GetKeyDown(KeyCode.J))
                animationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimationType.TAKE_DAMAGE);
            if (Input.GetKeyDown(KeyCode.K))
                animationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimationType.DEATH);
        }
    }
}