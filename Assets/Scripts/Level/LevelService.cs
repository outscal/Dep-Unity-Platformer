using System.Collections.Generic;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Level
{
    public class LevelService
    {
        private LevelConfiguration levelConfiguration;

        public LevelService(LevelConfiguration levelConfiguration)
        {
            this.levelScriptableObjects = levelScriptableObjects;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.AddListener(LoadLevel);

        private void UnsubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.RemoveListener(LoadLevel);

        private void LoadLevel(int levelID = 1)
        {
            var levelData = levelConfiguration.levelConfig.Find(levelSO => levelSO.ID == levelID);
            Object.Instantiate(levelData.LevelPrefab);
            UnsubscribeToEvents();
        }
    }
}