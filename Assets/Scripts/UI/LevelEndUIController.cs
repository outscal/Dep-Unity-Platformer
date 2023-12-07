using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.UI
{
    public class LevelEndUIController : IUIController
    {
        private LevelEndUIView levelEndView;
        
        public LevelEndUIController(LevelEndUIView levelEndView)
        {
            this.levelEndView = levelEndView;
            levelEndView.SetController(this);
            Hide();
        }

        public void Show() => levelEndView.EnableView();

        public void Hide() => levelEndView.DisableView();

        public void OnHomeButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void OnQuitButtonClicked() => Application.Quit();

        public void EndGame(bool playerWon){
            levelEndView.EndGame(playerWon);
            Show();
        }
    }
}