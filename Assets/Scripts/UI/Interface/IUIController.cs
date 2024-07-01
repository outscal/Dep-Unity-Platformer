namespace Platformer.UI
{
    public interface IUIController 
    {
        public void InitializeController(IUIView iuiView) { }

        public void Show();
        public void Hide();
    }
}