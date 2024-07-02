using System.Collections.Generic;
using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.Events;
using Platformer.Level;
using Platformer.Main;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Enemy
{
    public class EnemyService
    {
        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        private LevelService LevelService => GameService.Instance.LevelService;
        private UIService UIService => GameService.Instance.UIService;
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        #endregion

        #region Getters
        public int ActiveEnemiesCount => activeEnemies.Count;
        public int SpawnedEnemies { 
            get => spawnedEnemies;
            private set {
                spawnedEnemies = value;
                UIService.UpdateEnemyCountUI(ActiveEnemiesCount, spawnedEnemies);
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
                if(enemy is not SpikeController)
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
                EnemyType.Slime => new SlimeController(enemyScriptableObject),
                EnemyType.MushroomHead => new MushroomHeadController(enemyScriptableObject),
                _ => new EnemyController(enemyScriptableObject),
            };
            return enemy;
        }

        public void AddEnemy(EnemyController enemy) => activeEnemies.Add(enemy);

        public async void EnemyDied(EnemyController deadEnemy)
        {
            activeEnemies.Remove(deadEnemy);
            EventService.OnEnemyDied.InvokeEvent(deadEnemy);
            if (AllEnemiesDied()) 
            {
                await Task.Delay((int)deadEnemy.Data.DelayAfterGameEnd * 1000);
                EventService.OnAllEnemiesDied.InvokeEvent();
            }
        }

        public void UpdateEnemyHealth(EnemyController enemy, float healthRatio) => UIService.UpdateEnemyHealthUI(enemy, healthRatio);

        public void EnemyMoved(EnemyController enemyController) => EventService.OnEnemyMoved.InvokeEvent(enemyController);

        private bool AllEnemiesDied() => activeEnemies.Count == 0;

        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayMushroomHeadAttackAnimation(animator);
    }
}