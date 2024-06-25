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
                { KeyCode.K, PlayerInputTriggers.DEATH }, // temporary
                { KeyCode.J, PlayerInputTriggers.TAKE_DAMAGE } // temporary
            };
        }

        public void HandleInput() 
        {
            HandlePlayerMoevementInput();
            HandlePlayerTriggerInput();
        }

        private void HandlePlayerMoevementInput()
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

    public enum PlayerInputTriggers
    {
        JUMP, // SPACE
        ATTACK, // X
        SLIDE, // C
        DEATH, // K // temporary
        TAKE_DAMAGE // J // temporary
    }
}