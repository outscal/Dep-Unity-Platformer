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

        [Header("World Space Canvas")]
        [SerializeField] private WorldSpaceCanvasController worldSpaceCanvasController;

        [Header("Screen Space Overlay Canvas")]
        [SerializeField] private ScreenSpaceOverlayCanvasController screenSpaceOverlayCanvasController;

        private void Awake(){
            screenSpaceOverlayCanvasController.Initialise();
            worldSpaceCanvasController.Initialise();
        }

        private void Start() => SubscribeToEvents();

        private void SubscribeToEvents(){
            EventService.OnLevelSelected.AddListener(ShowGameplayUI);
            EventService.OnAllEnemiesDied.AddListener(OnAllEnemiesDied);
            EventService.OnEnemyDied.AddListener(OnEnemyDied);
            EventService.OnEnemyMoved.AddListener(OnEnemyMoved);
        }

        private void UnsubscribeToEvents(){
            EventService.OnLevelSelected.RemoveListener(ShowGameplayUI);
            EventService.OnAllEnemiesDied.RemoveListener(OnAllEnemiesDied);
            EventService.OnEnemyDied.RemoveListener(OnEnemyDied);
            EventService.OnEnemyMoved.RemoveListener(OnEnemyMoved);
        }

        #region Screen Space overlay
        public void ShowLevelSelectionUI(int levelCount) => screenSpaceOverlayCanvasController.ShowLevelSelectionUI(levelCount);
        private void ShowGameplayUI(int levelId) => screenSpaceOverlayCanvasController.ShowGameplayUI(levelId);
        public void ToggleKillOverlay(bool value) => screenSpaceOverlayCanvasController.ToggleKillOverlay(value);
        public void EndGame(bool playerWon) => screenSpaceOverlayCanvasController.EndGame(playerWon);
        public void UpdatePlayerHealthUI(float healthRatio) => screenSpaceOverlayCanvasController.UpdatePlayerHealth(healthRatio);
        public void UpdateEnemyCountUI(int activeEnemies, int totalEnemies) => screenSpaceOverlayCanvasController.UpdateEnemyCount(activeEnemies, totalEnemies);
        public void UpdateCoinsCountUI(int coinsCount) => screenSpaceOverlayCanvasController.UpdateCoinsCount(coinsCount);
        #endregion

        #region World Space
        public void UpdateEnemyHealthUI(EnemyController enemy, float healthRatio) => worldSpaceCanvasController.UpdateEnemyHealth(enemy, healthRatio);
        public void OnEnemyMoved(EnemyController enemyController) => worldSpaceCanvasController.OnEnemyMoved(enemyController);
        #endregion

        private void OnEnemyDied(EnemyController enemyController){
            screenSpaceOverlayCanvasController.EnemyDied(EnemyService.ActiveEnemiesCount, EnemyService.SpawnedEnemies);
            worldSpaceCanvasController.OnEnemyDied(enemyController);
        }

        private void OnAllEnemiesDied() => EndGame(true);

        private void OnDestroy() => UnsubscribeToEvents();
    }
}