using System.Collections.Generic;
using Platformer.Main;
using TMPro;
using UnityEngine;

namespace Platformer.Level
{
    public class LevelService
    {
        private List<LevelScriptableObject> levelScriptableObjects;

        public LevelService(List<LevelScriptableObject> levelScriptableObjects)
        {
            this.levelScriptableObjects = levelScriptableObjects;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.AddListener(LoadLevel);

        private void UnsubscribeToEvents() => GameService.Instance.EventService.OnLevelSelected.RemoveListener(LoadLevel);

        private void LoadLevel(int levelID = 1)
        {
            var levelData = levelScriptableObjects.Find(levelSO => levelSO.ID == levelID);
            Object.Instantiate(levelData.LevelPrefab);
            UnsubscribeToEvents();
        }

        public void BackgroundParallaxEffect(Transform[] sprites) => GameService.Instance.CameraService.BackgroundParallax(sprites);
    }
}