using Platformer.Main;
using System;

namespace Platformer.InputSystem{
    public class InputService{

        private KeyboardInputHandler keyboardInputHandler;

        // TODO: Why are these Keyboard input events not being invoked by the Input Controller directly?
        public static event Action<float> OnHorizontalAxisInputReceived;
        public static event Action<PlayerInputTriggers> OnPlayerTriggerInputReceived;

        public InputService() => keyboardInputHandler = new KeyboardInputHandler();

        public void UpdateInputService() => keyboardInputHandler.HandleInput();

        public void HandleHorizontalAxisInput(float horizontalInput) => OnHorizontalAxisInputReceived?.Invoke(horizontalInput);

        public void HandlePlayerTriggerInput(PlayerInputTriggers playerInputTrigger) => OnPlayerTriggerInputReceived?.Invoke(playerInputTrigger);
    }
}