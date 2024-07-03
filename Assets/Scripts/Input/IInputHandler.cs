namespace Platformer.InputSystem
{
    // TODO: We dont need a IInputHandler if we only have one type of input handler in this project as it will be over architecture like discussed before.
    // TODO: Input Service is directly referencing an instance of KeyboardInputHandler so there is no use of this interface as of now.
    public interface IInputHandler
    {
        public void HandleInput();
    }
}