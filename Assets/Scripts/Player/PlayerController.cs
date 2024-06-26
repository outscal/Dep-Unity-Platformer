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
    }
}