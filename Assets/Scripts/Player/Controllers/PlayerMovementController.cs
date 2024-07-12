using System.Collections;
using Platformer.Player.Enumerations;
using Platformer.Services;
using UnityEngine;

namespace Platformer.Player.Controllers
{
    public class PlayerMovementController
    {
        private PlayerController owner;
        private PlayerView playerView;
        private PlayerScriptableObject playerScriptableObject;
        
        private float currentSpeed;
        private int coroutineIdSlide;

        public PlayerMovementController(PlayerController owner, PlayerView playerView, PlayerScriptableObject playerScriptableObject)
        {
            this.owner = owner;
            this.playerView = playerView;
            this.playerScriptableObject = playerScriptableObject;
        }

        public void Update(float currentHorizontalInput)
        {
            HandleVerticalMovement();
            HandleHorizontalMovement(currentHorizontalInput);
        }
        
        private void HandleVerticalMovement()
        {
            if (isFalling())
            {
                //Increase Falling Velocity
                playerView.AddVelocity(playerScriptableObject.fallMultiplier * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
            else if (isJumping())
            {
                //Decrease Jumping Velocity
                playerView.AddVelocity(playerScriptableObject.lowJumpMultiplier * Physics2D.gravity.y * Time.deltaTime * Vector2.up);
            }
        }
        
        private void HandleHorizontalMovement(float currentHorizontalInput)
        {
            UpdateRunningState(currentHorizontalInput);
            TranslatePlayer(currentHorizontalInput);
        }

        private void UpdateRunningState(float currentHorizontalInput)
        {
            PlayerState newState = currentHorizontalInput != 0 ? PlayerState.RUNNING : PlayerState.IDLE;
            owner.SetPlayerState(newState);
        }
        
        private void TranslatePlayer(float currentHorizontalInput)
        {
            var movementVector = new Vector3(currentHorizontalInput, 0.0f, 0.0f).normalized;
            playerView.TranslatePlayer(currentSpeed * Time.deltaTime * movementVector);
            owner.PlayerMoved(playerView.Position);
        }
        

        // JUMP 1st IMPLEMENTATION
        public void Jump() => playerView.SetVelocity(Vector2.up * playerScriptableObject.jumpForce);

        // JUMP 2nd IMPLEMENTATION
        // public void Jump() => PlayerView.AddForce(Vector2.up * playerScriptableObject.jumpForce, ForceMode2D.Impulse);

        // JUMP 3rd IMPLEMENTATION
        /*private void Jump() // direct changing the position through translate method
        {
            var force = playerScriptableObject.jumpForce;
            float jumpHeight = force * Time.deltaTime; // Convert force to a height
            PlayerView.TranslatePlayer(Vector2.up * jumpHeight);
        }*/
        
        private bool isFalling() => playerView.GetVelocity().y < 0;
        
        private bool isJumping() => playerView.GetVelocity().y > 0;
        
        public bool CanSlide() => IsGrounded() && owner.GetPlayerState() == PlayerState.RUNNING;
        
        public bool IsGrounded()
        {
            var offset = 0.3f;
            var raycastHit = Physics2D.Raycast(playerView.GetColliderBounds().center, Vector2.down, playerView.GetColliderBounds().extents.y + offset, playerView.GroundLayer);
            return raycastHit.collider != null;
        }
        
        public void UpdateCurrentSpeed()
        {
            switch (owner.GetPlayerState())
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

        public void Slide(float currentHorizontalInput) => coroutineIdSlide = CoroutineService.StartCoroutine(SlideCoroutine(currentHorizontalInput));
        
        private IEnumerator SlideCoroutine(float currentHorizontalInput)
        {
            owner.SetPlayerState(PlayerState.SLIDE);
            yield return new WaitForSeconds(playerScriptableObject.slidingTime * 1000);
            
            // TODO: Check if this is even needed or if we can just simply set the player state back to Idle.
            ResetStateAfterSliding(currentHorizontalInput);
        }

        private void ResetStateAfterSliding(float currentHorizontalInput)
        {
            PlayerState newState = CanRun(currentHorizontalInput) ? PlayerState.RUNNING : PlayerState.IDLE;
            owner.SetPlayerState(newState);
        }
        
        public bool CanRun(float currentHorizontalInput) => owner.GetPlayerState() != PlayerState.SLIDE && currentHorizontalInput != 0;
        
        public bool CanJump() => IsGrounded() && (owner.GetPlayerState() == PlayerState.IDLE || owner.GetPlayerState() == PlayerState.RUNNING);
        
    }
}
