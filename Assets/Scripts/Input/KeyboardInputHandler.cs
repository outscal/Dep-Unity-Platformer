using System.Collections.Generic;
using Platformer.Cameras;
using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem
{
    public class KeyboardInputHandler : IInputHandler
    {
        private InputService InputService => GameService.Instance.InputService;

        private readonly Dictionary<KeyCode, PlayerInputTriggers> keyMappings;

        public KeyboardInputHandler()
        {
            keyMappings = new Dictionary<KeyCode, PlayerInputTriggers> {
                { KeyCode.Space, PlayerInputTriggers.JUMP },
                { KeyCode.X, PlayerInputTriggers.ATTACK },
                { KeyCode.C, PlayerInputTriggers.SLIDE }
            };
        }

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
            foreach (var mapping in keyMappings) {
                if (Input.GetKeyDown(mapping.Key)) {
                    InputService.HandlePlayerTriggerInput(mapping.Value);
                }
            }
        }

        private void HandleCameraControlInput()
        {
            if (Input.GetKey(KeyCode.Q))
                InputService.HandleCameraZoomInput(ZoomType.ZOOMOUT);
            else if (Input.GetKey(KeyCode.E))
                InputService.HandleCameraZoomInput(ZoomType.ZOOMIN);
        }
    }

    public enum PlayerInputTriggers
    {
        JUMP, // SPACE
        ATTACK, // X
        SLIDE // C
    }
}