using Platformer.AnimationSystem;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerController: MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private AnimationService animation_service;
        private PlayerState current_state;

        private void Awake()
        {
            animation_service = new AnimationService(animator);
            current_state = PlayerState.IDLE;
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
            // Check for movement input
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            
            // Set Current Player State
            current_state = (Mathf.Abs(horizontalInput) > 0.1f) ? PlayerState.RUNNING : PlayerState.IDLE;

            // PLay Movement Animations
            PlayMovementAnimation();
            
            // Flip Player Sprite if needed
            FlipSpriteIfNeeded(horizontalInput);
        }

        private void FlipSpriteIfNeeded(float horizontalInput)
        {
            if (current_state == PlayerState.RUNNING)
                transform.localScale = new Vector3(Mathf.Sign(horizontalInput), transform.localScale.y, transform.localScale.z);
        }

        private void PlayMovementAnimation()
        {
            animation_service.PlayPlayerMovementAnimation(current_state);
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
    }
}