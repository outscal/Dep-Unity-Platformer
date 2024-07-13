using Platformer.AnimationSystem;
using Platformer.Game;
using Platformer.Main;
using Platformer.UI;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerService
    {
        private PlayerController playerController;
        private readonly PlayerScriptableObject playerScriptableObject;

        public PlayerService(PlayerScriptableObject playerScriptableObject)
        {
            this.playerScriptableObject = playerScriptableObject;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => LevelSelectionUIController.OnLevelSelected += SpawnPlayer;

        private void UnsubscribeToEvents() => LevelSelectionUIController.OnLevelSelected -= SpawnPlayer;

        private void SpawnPlayer(int levelId)
        {
            playerController = new PlayerController(playerScriptableObject);
            UnsubscribeToEvents();
        }

        public void Update() => playerController?.Update();
    }
}