using Platformer.Cameras;
using System;

namespace Platformer.InputSystem{
    public class InputService{

        private KeyboardInputHandler keyboardInputHandler;
        
        //AXIS INPUT
        public static event Action<float> OnHorizontalAxisInputReceived;
        public static event Action<float> OnVerticalAxisInputReceived;
        
        //TRIGGER INPUT
        public static event Action<PlayerInputTriggers> OnPlayerTriggerInputReceived;
        
        //CAMERA INPUT
        public static event Action<ZoomType> OnCameraZoomInputReceived;

        public InputService() => keyboardInputHandler = new KeyboardInputHandler();

        public void Update() => keyboardInputHandler.HandleInput();

        public void HandleHorizontalAxisInput(float horizontalInput) => OnHorizontalAxisInputReceived?.Invoke(horizontalInput);

        public void HandleCameraZoomInput(ZoomType zoomType) => OnCameraZoomInputReceived?.Invoke(zoomType);
        
        public void HandleVerticalAxisInput(float verticalInput) => OnVerticalAxisInputReceived?.Invoke(verticalInput);

        public void HandleTriggerInput(PlayerInputTriggers playerInputTrigger) => OnPlayerTriggerInputReceived?.Invoke(playerInputTrigger);
        
    }
}