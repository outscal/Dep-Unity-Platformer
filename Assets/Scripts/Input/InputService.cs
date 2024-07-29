using Platformer.Main;
using System;

namespace Platformer.InputSystem
{
    public class InputService
    {
        private KeyboardInputHandler keyboardInputHandler;

        public static event Action<float> OnHorizontalAxisInputReceived;
        public static event Action<PlayerInputTriggers> OnPlayerTriggerInputReceived;

        public InputService() => keyboardInputHandler = new KeyboardInputHandler();

        public void Update() => keyboardInputHandler.HandleInput();

        public void HorizontalAxisInputReceived(float horizontalInput) => OnHorizontalAxisInputReceived?.Invoke(horizontalInput);

        public void PlayerTriggerInputReceived(PlayerInputTriggers playerInputTrigger) => OnPlayerTriggerInputReceived?.Invoke(playerInputTrigger);
    }
}