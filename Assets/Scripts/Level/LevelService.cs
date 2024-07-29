using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Level
{
    public class LevelService
    {
        private LevelConfiguration levelConfiguration;

        public LevelService(LevelConfiguration levelConfiguration)
        {
            this.levelConfiguration = levelConfiguration;
            LoadLevel();
        } 

        public void LoadLevel(int levelID = 1)
        {
            var levelData = levelConfiguration.levelConfig.Find(levelSO => levelSO.ID == levelID);
            Object.Instantiate(levelData.LevelPrefab);
        }
    }
}