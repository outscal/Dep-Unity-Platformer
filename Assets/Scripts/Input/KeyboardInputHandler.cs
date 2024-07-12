using System.Collections.Generic;
using Platformer.Cameras;
using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem
{
    public class KeyboardInputHandler
    {
        private InputService InputService => GameService.Instance.InputService;

        private Dictionary<KeyCode, PlayerInputTriggers> keyMappings;

        public KeyboardInputHandler() => CreateTriggerMappings();

        private void CreateTriggerMappings()
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
                    InputService.HandleTriggerInput(mapping.Value);
                }
            }
        }

        private void HandleCameraControlInput()
        {
            if (Input.GetKey(KeyCode.Q)) //ZoomOut
                InputService.HandleCameraZoomInput(ZoomType.ZOOMOUT);
            else if (Input.GetKey(KeyCode.E)) //ZoomIn
                InputService.HandleCameraZoomInput(ZoomType.ZOOMIN);
        }
    }
}