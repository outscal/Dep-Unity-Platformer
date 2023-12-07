using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem{
    public class KeyboardInputHandler : IInputHandler
    {
        private InputService inputService => GameService.Instance.InputService;

        public void HandleInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            if(Input.GetKeyDown(KeyCode.C)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.JUMP);
            }else if(Input.GetKeyDown(KeyCode.X)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.ATTACK);
            }else if(Input.GetKeyDown(KeyCode.S)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.SLIDE);
            }
            inputService.HandleHorizontalAxisInput(horizontalInput);
        }
    }

    public enum PlayerInputTriggers{
        JUMP, // C
        ATTACK, // X
        SLIDE, // S
    }
}