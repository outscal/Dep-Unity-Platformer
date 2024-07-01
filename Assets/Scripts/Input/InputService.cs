using Platformer.Cameras;
using Platformer.Events;
using Platformer.Main;

namespace Platformer.InputSystem{
    public class InputService{

        private readonly KeyboardInputHandler keyboardInputHandler;
        private EventService EventService => GameService.Instance.EventService;

        public InputService() => keyboardInputHandler = new KeyboardInputHandler();

        public void UpdateInputService() => keyboardInputHandler.HandleInput();

        public void HandleHorizontalAxisInput(float horizontalInput) => EventService.OnHorizontalAxisInputReceived.InvokeEvent(horizontalInput);

        public void HandleCameraZoomInput(ZoomType zoomType) => EventService.OnCameraZoomInputReceived.InvokeEvent(zoomType);
        
        public void HandleVerticalAxisInput(float verticalInput) => EventService.OnVerticalAxisInputReceived.InvokeEvent(verticalInput);

        public void HandlePlayerTriggerInput(PlayerInputTriggers playerInputTrigger) => EventService.OnPlayerTriggerInputReceived.InvokeEvent(playerInputTrigger);
    }
}