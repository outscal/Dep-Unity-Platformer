namespace Platformer.UI
{
    public class GameplayUIController : IUIController
    {
        private GameplayUIView gameplayView;
        private const string ENEMY_COUNTER_PREFIX = "Enemies Left:";
        private const string PLAYER_COINS_COLLECTED = "Coins Collected:";

        public GameplayUIController(GameplayUIView gameplayView)
        {
            InitializeController(gameplayView);
        }
        public void InitializeController(IUIView iuiView)
        {
            gameplayView = iuiView as GameplayUIView;
            gameplayView.SetController(this);
            Hide();
        }

        public void ShowLevel(int levelId){
            gameplayView.SetLevelText(levelId);
            Show();
        }

        public void Show() => gameplayView.EnableView(); 

        public void Hide() => gameplayView.DisableView();

        public void SetEnemyCount(int activeEnemies, int totalEnemies) => gameplayView.UpdateEnemyCounterText($"{ENEMY_COUNTER_PREFIX} {activeEnemies} / {totalEnemies}");

        public void SetPlayerHealthUI(float healthRatio) => gameplayView.UpdatePlayerHealthUI(healthRatio);

        public void SetCoinsCount(int coinsCollected) => gameplayView.UpdateCoinsCollectedUI($"{PLAYER_COINS_COLLECTED} {coinsCollected}");

        public void ToggleKillOverlay(bool value) => gameplayView.ToggleKillOverlay(value);

        
    }
}