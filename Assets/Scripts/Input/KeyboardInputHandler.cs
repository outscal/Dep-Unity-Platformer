using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem{
    public class KeyboardInputHandler : IInputHandler
    {
        private InputService inputService => GameService.Instance.InputService;

        public void HandleInput()
        {
            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");
            if(Input.GetKeyDown(KeyCode.C)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.JUMP);
            }else if(Input.GetKeyDown(KeyCode.X)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.ATTACK);
            }else if(Input.GetKeyDown(KeyCode.D)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.DEATH);
            }else if(Input.GetKeyDown(KeyCode.S)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.SLIDE);
            }else if(Input.GetKeyDown(KeyCode.A)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.TAKE_DAMAGE);
            }
            inputService.HandleHorizontalAxisInput(horizontalInput);
            inputService.HandleVerticalAxisInput(verticalInput);
        }
    }

    public enum PlayerInputTriggers{
        JUMP, // C
        ATTACK, // X
        DEATH, // D
        SLIDE, // S
        TAKE_DAMAGE // A
    }
}