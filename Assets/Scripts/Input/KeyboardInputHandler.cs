using Platformer.Main;
using UnityEngine;

namespace Platformer.InputSystem{
    public class KeyboardInputHandler : IInputHandler
    {
        private InputService InputService => GameService.Instance.InputService;

        public void HandleInput()
        {
            var horizontalInput = Input.GetAxisRaw("Horizontal");
            if(Input.GetKeyDown(KeyCode.Space)){
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.JUMP);
            }else if(Input.GetKeyDown(KeyCode.X)){
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.ATTACK);
            }else if(Input.GetKeyDown(KeyCode.C)){
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.SLIDE);
            }else if(Input.GetKeyDown(KeyCode.J)){
                InputService.HandlePlayerTriggerInput(PlayerInputTriggers.TAKE_DAMAGE);
            }
            InputService.HandleHorizontalAxisInput(horizontalInput);
        }
    }

    public enum PlayerInputTriggers{
        JUMP, // SPACE
        ATTACK, // X
        SLIDE, // C
        TAKE_DAMAGE // J // temporary
    }
}