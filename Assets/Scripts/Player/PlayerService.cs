using Platformer.AnimationSystem;
using Platformer.Cameras;
using Platformer.Game;
using Platformer.Main;
using Platformer.UI;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerService
    {
        #region Service References
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        private EventService EventService => GameService.Instance.EventService;
        private UIService UIService => GameService.Instance.UIService;
        private CameraService CameraService => GameService.Instance.CameraService;
        #endregion


        
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
            PlayTakeDamageAnimation(animator);
        }
        private void UnsubscribeToEvents() => LevelSelectionUIController.OnLevelSelected -= SpawnPlayer;

        public void TakeDamage(int damageToInflict) => playerController.TakeDamage(damageToInflict);

        private void SpawnPlayer(int levelId)
        {
            playerController = new PlayerController(playerScriptableObject);
            UnsubscribeToEvents();
        }

        public void Update() => playerController?.Update();

        ~PlayerService()
        {
            playerController = null;
        }
    }
}