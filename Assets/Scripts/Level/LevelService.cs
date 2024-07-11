using System;
using System.Collections.Generic;
using Platformer.Main;
using Platformer.UI;
using Platformer.Utilities;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Platformer.Level
{
    public class LevelService
    {
        private LevelConfiguration levelConfiguration;

        public LevelService(LevelConfiguration levelConfiguration)
        {
            this.levelConfiguration = levelConfiguration;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => LevelSelectionUIController.OnLevelSelected += LoadLevel;

        private void UnsubscribeToEvents() => LevelSelectionUIController.OnLevelSelected -= LoadLevel;

        private void LoadLevel(int levelID = 1)
        {
            var levelData = levelConfiguration.levelConfig.Find(levelSO => levelSO.ID == levelID);
            Object.Instantiate(levelData.LevelPrefab);
            UnsubscribeToEvents();
        }

        ~LevelService() => UnsubscribeToEvents();
    }
}