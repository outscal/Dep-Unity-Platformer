using Platformer.Main;
using System;

namespace Platformer.InputSystem{
    public class InputService{

        private KeyboardInputHandler keyboardInputHandler;

        public static event Action<float> OnHorizontalAxisInputReceived;
        public static event Action<PlayerInputTriggers> OnPlayerTriggerInputReceived;

        public InputService() => keyboardInputHandler = new KeyboardInputHandler();

        public void UpdateInputService() => keyboardInputHandler.HandleInput();

        public void HandleHorizontalAxisInput(float horizontalInput) => OnHorizontalAxisInputReceived?.Invoke(horizontalInput);

        public void HandlePlayerTriggerInput(PlayerInputTriggers playerInputTrigger) => OnPlayerTriggerInputReceived?.Invoke(playerInputTrigger);
    }
}