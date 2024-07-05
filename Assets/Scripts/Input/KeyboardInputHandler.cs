using System.Collections.Generic;
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
            InputService.HorizontalAxisInputReceived(horizontalInput);
        }

        private void HandlePlayerTriggerInput()
        {
            foreach (var mapping in keyMappings) {
                if (Input.GetKeyDown(mapping.Key)) {
                    InputService.PlayerTriggerInputReceived(mapping.Value);
                }
            }
        }
    }
}