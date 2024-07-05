using System.Collections.Generic;
using Platformer.Drop;
using Platformer.Enemy;
using Platformer.Main;
using Platformer.Sound;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Level
{
    public class LevelService
    {
        #region Service Refrences
        private UIService UIService => GameService.Instance.UIService;
        private SoundService SoundService => GameService.Instance.SoundService;
        #endregion

        private List<LevelScriptableObject> levelScriptableObjects;

        private LevelScriptableObject currentLevelData;

        private bool CanOpenGate = false;

        public LevelService(List<LevelScriptableObject> levelScriptableObjects)
        {
            this.levelScriptableObjects = levelScriptableObjects;
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            GameService.Instance.EventService.OnLevelSelected.AddListener(LoadLevel);
            GameService.Instance.EventService.OnDropCollected.AddListener(CheckForLevelGateCondition);
        }

        private void UnsubscribeToEvents(){
            GameService.Instance.EventService.OnLevelSelected.RemoveListener(LoadLevel);
            GameService.Instance.EventService.OnDropCollected.RemoveListener(CheckForLevelGateCondition);
        }

        private void LoadLevel(int levelID = 1)
        {
            currentLevelData = levelScriptableObjects.Find(levelSO => levelSO.ID == levelID);
            Object.Instantiate(currentLevelData.LevelPrefab);
            //UnsubscribeToEvents();
        }

        private void CheckForLevelGateCondition(DropType dropType, int numberOfDropsCollected) => CanOpenGate = currentLevelData.LevelProgression.CheckGateOpenCondition(dropType, numberOfDropsCollected);

        public void PlayerReachedGate(){
            if(CanOpenGate){
                SoundService.PlaySoundEffects(SoundType.GAME_WON);
                UIService.EndGame(true);
                UnsubscribeToEvents();
            }
        }

        public void BackgroundParallaxEffect(Transform[] sprites) => GameService.Instance.CameraService.BackgroundParallax(sprites);
        public List<EnemyScriptableObject> GetEnemyDataForLevel(int levelId) => levelScriptableObjects.Find(level => level.ID == levelId).EnemyScriptableObjects;
        public List<DropScriptableObject> GetDropsDataForLevel(int levelId) => levelScriptableObjects.Find(level => level.ID == levelId).DropScriptableObjects;
    }
}