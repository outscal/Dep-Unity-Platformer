using System;
using Platformer.Cameras;
using Platformer.Main;
using Platformer.UI;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerService
    {
        #region Service References
        private CameraService CameraService => GameService.Instance.CameraService;
        #endregion

        public static event Action<Vector3> OnPlayerMoved;
        
        private PlayerScriptableObject playerScriptableObject;
        
        public PlayerController playerController { get; private set; }
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => LevelSelectionUIController.OnLevelSelected += SpawnPlayer;

        public void TakeDamage(Animator animator)
        {
            // use the camera shake effect
            CameraService.ShakeCamera();
        }
        private void UnsubscribeToEvents() => LevelSelectionUIController.OnLevelSelected -= SpawnPlayer;

        public void TakeDamage(int damageToInflict) => playerController.TakeDamage(damageToInflict);

        private void SpawnPlayer(int levelId)
        {
            playerController = new PlayerController(playerScriptableObject, this);
            UnsubscribeToEvents();
        }

        public void Update() => playerController?.Update();

        //Invoking Events 
        public void PlayerMoved(Vector3 newPosition) => OnPlayerMoved?.Invoke(newPosition);
            
        ~PlayerService()
        {
            playerController = null;
        }
        
    }
}