using System.Collections.Generic;
using System.Threading.Tasks;
using Platformer.Events;
using Platformer.Level;
using Platformer.Main;
using Platformer.UI;

namespace Platformer.Enemy
{
    public class EnemyService
    {
        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        private LevelService LevelService => GameService.Instance.LevelService;
        private UIService UIService => GameService.Instance.UIService;
        #endregion

        #region Getters
        public int ActiveEnemiesCount => activeEnemies.Count;
        public int SpawnedEnemies { 
            get => spawnedEnemies;
            private set {
                spawnedEnemies = value;
                UIService.UpdateEnemyCount(ActiveEnemiesCount, spawnedEnemies);
            } 
        }
        #endregion

        private List<EnemyController> activeEnemies;
        private int spawnedEnemies;

        public EnemyService()
        {
            InitializeVariables();
            SubscribeToEvents();
        }

        private void InitializeVariables() => activeEnemies = new List<EnemyController>();

        private void SubscribeToEvents() => EventService.OnLevelSelected.AddListener(SpawnEnemies);

        private void UnsubscribeToEvents() => EventService.OnLevelSelected.RemoveListener(SpawnEnemies);

        public void SpawnEnemies(int levelId)
        {
            var enemyDataForLevel = LevelService.GetEnemyDataForLevel(levelId);
            
            foreach(var enemySO in enemyDataForLevel)
            {
                var enemy = CreateEnemy(enemySO);
                AddEnemy(enemy);
            }

            SpawnedEnemies = activeEnemies.Count;
            UnsubscribeToEvents();
        }

        public EnemyController CreateEnemy(EnemyScriptableObject enemyScriptableObject)
        {
            EnemyController enemy = enemyScriptableObject.Type switch
            {
                EnemyType.Spike => new SpikeController(enemyScriptableObject),
                EnemyType.PatrolMan => new PatrolmanController(enemyScriptableObject),
                _ => new EnemyController(enemyScriptableObject),
            };
            return enemy;
        }

        public void AddEnemy(EnemyController enemy) => activeEnemies.Add(enemy);

        public async void EnemyDied(EnemyController deadEnemy)
        {
            activeEnemies.Remove(deadEnemy);
            // sound effect -- event 
            EventService.OnEnemyDied.InvokeEvent();
            if (AllEnemiesDied()) 
            {
                await Task.Delay((int)deadEnemy.Data.DelayAfterGameEnd * 1000); //converting seconds to milliseconds 
                // sound effect // event -- player won condition met
                EventService.OnAllEnemiesDied.InvokeEvent();
            }
        }

        private bool AllEnemiesDied() => activeEnemies.Count == 0;
    }
}