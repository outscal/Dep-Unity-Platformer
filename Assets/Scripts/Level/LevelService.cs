using System;
using System.Collections.Generic;
using Platformer.Main;
using Platformer.UI;
using Platformer.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Platformer.Level
{
    public class LevelService
    {
        private LevelConfiguration levelConfiguration;
        public static event Action OnLevelEnd;

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
            GameService.Instance.CameraService.SetCameraBounds(levelData.CameraBounds);
            UnsubscribeToEvents();
        }

        public void RestartLevel()
        {
            OnLevelEnd?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void BackgroundParallaxEffect(Transform[] sprites) => GameService.Instance.CameraService.BackgroundParallax(sprites);
        ~LevelService() => UnsubscribeToEvents();
    }
}