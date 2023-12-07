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

            #region Player Trigger Controls
            if(Input.GetKeyDown(KeyCode.C)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.JUMP);
            }else if(Input.GetKeyDown(KeyCode.X)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.ATTACK);
            }else if(Input.GetKeyDown(KeyCode.S)){
                inputService.HandlePlayerTriggerInput(PlayerInputTriggers.SLIDE);
            }
            #endregion

            #region Camera Controls
            if(Input.GetKey(KeyCode.Z)){
                inputService.HandleCameraZoomInput(false);

            }else if(Input.GetKey(KeyCode.A)){
                inputService.HandleCameraZoomInput(true);
            }
            #endregion

            inputService.HandleHorizontalAxisInput(horizontalInput);
            inputService.HandleVerticalAxisInput(verticalInput);
        }
    }

    public enum PlayerInputTriggers{
        JUMP, // C
        ATTACK, // X
        SLIDE, // S
    }
}