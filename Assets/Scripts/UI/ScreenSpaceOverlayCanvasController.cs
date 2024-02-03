using Platformer.Main;
using UnityEngine;

namespace Platformer.UI{
    public class ScreenSpaceOverlayCanvasController : MonoBehaviour
    {
        #region Service Reference
        private UIService UIService => GameService.Instance.UIService;
        #endregion

        [Header("Level Selection UI")]
        private LevelSelectionUIController levelSelectionController;
        [SerializeField] private LevelSelectionUIView levelSelectionView;
        [SerializeField] private LevelButtonView levelButtonPrefab;

        [Header("Level ENd UI")]
        private LevelEndUIController levelEndController;
        [SerializeField] private LevelEndUIView levelEndView;

        [Header("Gameplay UI")]
        private GameplayUIController gameplayController;
        [SerializeField] private GameplayUIView gameplayView;


        public void Initialise(){
            levelSelectionController = new LevelSelectionUIController(levelSelectionView, levelButtonPrefab);
            levelEndController = new LevelEndUIController(levelEndView);
            gameplayController = new GameplayUIController(gameplayView);
        }

        public void ShowLevelSelectionUI(int levelCount) => levelSelectionController.Show(levelCount);
        public void ShowGameplayUI(int levelId) => gameplayController.Show();
        public void ToggleKillOverlay(bool value) => gameplayController.ToggleKillOverlay(value);

        public void EndGame(bool playerWon){
            gameplayController.Hide();
            levelEndController.EndGame(playerWon);
        }

        public void UpdatePlayerHealth(float healthRatio) => gameplayController.SetPlayerHealthUI(healthRatio);

        public void UpdateCoinsCount(int coinsCount) => gameplayController.SetCoinsCount(coinsCount);

        public void UpdateEnemyCount(int activeEnemies, int totalEnemies) => gameplayController.SetEnemyCount(activeEnemies, totalEnemies);

        public void EnemyDied(int activeEnemies, int totalEnemies) => gameplayController.SetEnemyCount(activeEnemies, totalEnemies);
    }
}
