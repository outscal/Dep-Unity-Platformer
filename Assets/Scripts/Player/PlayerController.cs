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
                animationService = new AnimationService();
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

        private void PlayMovementAnimation()
        {
            animationService.ToggleBoolAnimation(animator, PlayerAnimationType.RUNNING.ToString(), currentState == PlayerState.RUNNING);
        }

        private void HandleTriggerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                animationService.PlayTriggerAnimation(animator, PlayerTriggerAnimationType.JUMP.ToString());
            if (Input.GetKeyDown(KeyCode.C))
                animationService.PlayTriggerAnimation(animator, PlayerTriggerAnimationType.SLIDE.ToString());
            if (Input.GetKeyDown(KeyCode.X))
                animationService.PlayTriggerAnimation(animator, PlayerTriggerAnimationType.ATTACK.ToString());
            if (Input.GetKeyDown(KeyCode.J))
                animationService.PlayTriggerAnimation(animator, PlayerTriggerAnimationType.TAKE_DAMAGE.ToString());
            if (Input.GetKeyDown(KeyCode.K))
                animationService.PlayTriggerAnimation(animator, PlayerTriggerAnimationType.DEATH.ToString());
        }
    }
}