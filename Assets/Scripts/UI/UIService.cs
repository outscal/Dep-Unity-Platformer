using Platformer.Enemy;
using Platformer.Events;
using Platformer.Main;
using UnityEngine;

namespace Platformer.UI
{
    public class UIService : MonoBehaviour
    {

        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        private EnemyService EnemyService => GameService.Instance.EnemyService;
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

        private void Start()
        {
            levelSelectionController = new LevelSelectionUIController(levelSelectionView, levelButtonPrefab);
            levelEndController = new LevelEndUIController(levelEndView);
            gameplayController = new GameplayUIController(gameplayView);
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            EventService.OnLevelSelected.AddListener(ShowGameplayUI);
            EventService.OnAllEnemiesDied.AddListener(OnAllEnemiesDied);
            EventService.OnEnemyDied.AddListener(OnEnemyDied);
        }

        private void UnsubscribeToEvents(){
            EventService.OnLevelSelected.RemoveListener(ShowGameplayUI);
            EventService.OnAllEnemiesDied.RemoveListener(OnAllEnemiesDied);
            EventService.OnEnemyDied.RemoveListener(OnEnemyDied);
        }

        public void ShowLevelSelectionUI(int levelCount) => levelSelectionController.Show(levelCount);

        private void ShowGameplayUI(int levelId) => gameplayController.Show();

        public void ToggleKillOverlay(bool value) => gameplayController.ToggleKillOverlay(value);

        public void EndGame(bool playerWon){
            gameplayController.Hide();
            levelEndController.EndGame(playerWon);
        }

        public void UpdatePlayerHealth(float healthRatio) => gameplayController.SetPlayerHealthUI(healthRatio);

        private void OnEnemyDied(EnemyController deadEnemy) => gameplayController.SetEnemyCount(EnemyService.ActiveEnemiesCount, EnemyService.SpawnedEnemies);

        private void OnAllEnemiesDied() => EndGame(true);

        public void UpdateEnemyCount(int activeEnemies, int totalEnemies) => gameplayController.SetEnemyCount(activeEnemies, totalEnemies);

        public void UpdateCoinsCount(int coinsCount) => gameplayController.SetCoinsCount(coinsCount);

        private void OnDestroy() => UnsubscribeToEvents();
    }
}