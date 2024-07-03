using System.Collections.Generic;
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
                { KeyCode.C, PlayerInputTriggers.SLIDE },
                { KeyCode.J, PlayerInputTriggers.TAKE_DAMAGE } // temporary
            };
        }

        public void HandleInput() 
        {
            HandlePlayerMovementInput();
            HandlePlayerTriggerInput();
        }

        private void HandlePlayerMovementInput()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            InputService.HandleHorizontalAxisInput(horizontalInput);
        }

        private void HandlePlayerTriggerInput()
        {
            foreach (var mapping in keyMappings) {
                if (Input.GetKeyDown(mapping.Key)) {
                    InputService.HandlePlayerTriggerInput(mapping.Value);
                }
            }
        }
    }

    // TODO: Create separate file for this Enum inside Input namespace 
    public enum PlayerInputTriggers
    {
        JUMP, // SPACE
        ATTACK, // X
        SLIDE, // C
        TAKE_DAMAGE // J // temporary
    }
}