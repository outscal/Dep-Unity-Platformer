using Platformer.Game;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    public class LevelEndUIController : IUIController
    {
        private LevelEndUIView levelEndView;
        
        public LevelEndUIController(LevelEndUIView levelEndView)
        {
            InitializeController(levelEndView);
        }
        public void InitializeController(IUIView iuiView) 
        {
            levelEndView = iuiView as LevelEndUIView;
            levelEndView.SetController(this);
            Hide();
        }
        public void Show() => levelEndView.EnableView();

        public void Hide() => levelEndView.DisableView();

        public void OnHomeButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void OnQuitButtonClicked() => Application.Quit();

        public void EndGame(GameEndType gameEndType){
            levelEndView.EndGame(gameEndType);
            Show();
        }
    }
}