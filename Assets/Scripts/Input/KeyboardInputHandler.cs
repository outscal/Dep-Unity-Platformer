using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem{
    public class KeyboardInputHandler : IInputHandler
    {
        private InputService InputService => GameService.Instance.InputService;

        public void HandleInput()
        {
            HandlePlayerMovementInput();
            HandlePlayerTriggerInput();
            HandleCameraControlInput();
        }

        private void HandlePlayerMovementInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            InputService.HandleHorizontalAxisInput(horizontalInput);
            InputService.HandleVerticalAxisInput(verticalInput);
        }

        private void HandlePlayerTriggerInput()
        {
            if (Input.GetKeyDown(KeyCode.Space))
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.JUMP);
            else if (Input.GetKeyDown(KeyCode.X))
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.ATTACK);
            else if (Input.GetKeyDown(KeyCode.C))
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.SLIDE);
        }

        private void HandleCameraControlInput()
        {
            if (Input.GetKey(KeyCode.Q))
                InputService.HandleCameraZoomInput(false);
            else if (Input.GetKey(KeyCode.E))
                InputService.HandleCameraZoomInput(true);
        }
    }

    public enum PlayerInputTriggers{
        JUMP, // SPACE
        ATTACK, // X
        SLIDE // C
    }
}