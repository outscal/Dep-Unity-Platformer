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
            var levelEnemySpawnConfig = LevelService.GetLevelEnemySpawnConfigForLevel(levelId);
            var enemyConfigurations = GameService.Instance.enemyConfiguration;
            
            foreach (var spawnData in levelEnemySpawnConfig.enemySpawnDataList)
            {
                var enemySO = GetEnemyScriptableObject(spawnData.EnemyType, enemyConfigurations);
                if (enemySO != null)
                {
                    var enemy = CreateEnemy(enemySO, spawnData);
                    if (enemy is not SpikeController)
                        AddEnemy(enemy);
                }
            }

            SpawnedEnemies = activeEnemies.Count;
            UnsubscribeToEvents();
        }
        
        public EnemyController CreateEnemy(EnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData)
        {
            switch (enemyScriptableObject.enemyType)
            {
                case EnemyType.Spike:
                    return new SpikeController(enemyScriptableObject, spawnData);
                
                case EnemyType.Slime:
                    if (enemyScriptableObject is SlimeScriptableObject slimeScriptableObject)
                    {
                        return new SlimeController(slimeScriptableObject, spawnData);
                    }
                    return null;
                
                case EnemyType.MushroomHead: 
                    if (enemyScriptableObject is MushroomHeadScriptableObject muhsroomHeadScriptableObject)
                    {
                        return new MushroomHeadController(muhsroomHeadScriptableObject, spawnData);
                    }
                    return null;
                
                default: 
                    return new EnemyController(enemyScriptableObject, spawnData);
            }
        }
        
        private EnemyScriptableObject GetEnemyScriptableObject(EnemyType enemyType, EnemyConfiguration enemyConfigurations)
        {
            foreach (var config in enemyConfigurations.enemyConfigurations)
            {
                if (config.enemyType == enemyType)
                {
                    return config.enemyScriptableObject;
                }
            }
            return null;
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
        
        public void Update()
        {
            foreach (var enemy in activeEnemies)
            {
                enemy.Update();
            }
        }

        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayMushroomHeadAttackAnimation(animator);
    }
}