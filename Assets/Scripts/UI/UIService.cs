using Platformer.Game;
using Platformer.Player;
using UnityEngine;

namespace Platformer.UI
{
    public class UIService : MonoBehaviour
    {
        [Header("Level Selection UI")]
        private LevelSelectionUIController levelSelectionUIController;
        [SerializeField] private LevelSelectionUIView levelSelectionView;
        [SerializeField] private LevelButtonView levelButtonPrefab;

        [Header("Level End UI")]
        private LevelEndUIController levelEndUIController;
        [SerializeField] private LevelEndUIView levelEndView;

        [Header("Gameplay UI")]
        private GameplayUIController gameplayUIController;
        [SerializeField] private GameplayUIView gameplayView;

        private void Awake()
        {
            levelSelectionUIController = new LevelSelectionUIController(levelSelectionView, levelButtonPrefab);
            levelEndUIController = new LevelEndUIController(levelEndView);
            gameplayUIController = new GameplayUIController(gameplayView);
            gameplayUIController.Hide();
            SubscribeToEvents();
        }

        private void SubscribeToEvents() 
        {
            LevelSelectionUIController.OnLevelSelected += ShowGameplayUI;
            PlayerController.OnGameEnd += EndGame;

        }

        private void UnsubscribeToEvents() 
        {
            LevelSelectionUIController.OnLevelSelected -= ShowGameplayUI;
            PlayerController.OnGameEnd -= EndGame;
        }

        public void CreateAndShowLevelSelectionUI(int levelCount) => levelSelectionUIController.CreateAndShowLevelSelection(levelCount);
        
        private void ShowGameplayUI(int levelId) => gameplayUIController.ShowLevel(levelId);

        public void ToggleKillOverlay(bool value) => gameplayUIController.ToggleKillOverlay(value);

        public void EndGame(GameEndType gameEndType){
            gameplayUIController.Hide();
            levelEndUIController.EndGame(gameEndType);
        }

        public void UpdatePlayerHealthUI(float healthRatio) => gameplayUIController.SetPlayerHealthUI(healthRatio);

        public void UpdateEnemyCount(int activeEnemies, int totalEnemies) => gameplayUIController.SetEnemyCount(activeEnemies, totalEnemies);

        public void UpdateCoinsCountUI(int coinsCount) => gameplayUIController.SetCoinsCount(coinsCount);

        private void OnDestroy() => UnsubscribeToEvents();
    }
}