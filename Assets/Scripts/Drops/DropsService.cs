using Platformer.Enemy;
using Platformer.Events;
using Platformer.Level;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Drop{
    public class DropsService{
        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        private LevelService LevelService => GameService.Instance.LevelService;
        #endregion

        public DropsService() => SubscribeToEvents();

        private void SubscribeToEvents(){
            EventService.OnLevelSelected.AddListener(SpawnDrops);
            EventService.OnEnemyDied.AddListener(OnEnemyDeath);
            EventService.OnAllEnemiesDied.AddListener(OnAllEnemiesDied);
        }

        private void UnsubscribeToEvents() => EventService.OnLevelSelected.RemoveListener(SpawnDrops);

        private void SpawnDrops(int levelId){
            var spawnDataForLevel = LevelService.GetDropsDataForLevel(levelId);
            foreach(var dropSO in spawnDataForLevel)
            {
                CreateDrop(dropSO);
            }
            UnsubscribeToEvents();
        }

        private void OnEnemyDeath(EnemyController deadEnemy){
            var drops = deadEnemy.Data.DropScriptableObjects;
            foreach(var drop in drops){
                CreateDrop(drop, deadEnemy.EnemyView.transform);
            }
        }

        private void OnAllEnemiesDied(){
            EventService.OnEnemyDied.RemoveListener(OnEnemyDeath);
            EventService.OnAllEnemiesDied.RemoveListener(OnAllEnemiesDied);
        }

        public void CreateDrop(DropScriptableObject dropScriptableObject, Transform parentTransform = null) => _ = new DropController(dropScriptableObject, parentTransform);
    }
}