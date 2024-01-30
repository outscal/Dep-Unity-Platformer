using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.Cameras;
using Platformer.Events;
using Platformer.Main;
using Platformer.UI;
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
        public PlayerController PlayerController { get; private set; }

        
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => EventService.OnLevelSelected.AddListener(SpawnPlayer);

        private void UnsubscribeToEvents(){
            EventService.OnLevelSelected.RemoveListener(SpawnPlayer);
            EventService.OnHorizontalAxisInputReceived.RemoveListener(PlayerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.RemoveListener(PlayerController.HandleTriggerInput);
        }

        private void SpawnPlayer(int levelId){
            PlayerController = new PlayerController(playerScriptableObject);
            EventService.OnHorizontalAxisInputReceived.AddListener(PlayerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.AddListener(PlayerController.HandleTriggerInput);
        }

        public void MovePlayer(Animator animator, bool isRunning, Vector3 playerPosition){
            PlayMovementAnimation(animator, isRunning);
            EventService.OnPlayerMoved.InvokeEvent(playerPosition);
        }

        public void TakeDamage(Animator animator){
            // use the camera shake effect
            CameraService.ShakeCamera();
            PlayTakeDamageAnimation(animator);
        }

        public async void PlayerDied(Animator animator){
            UnsubscribeToEvents();
            CameraService.ShakeCamera();
            PlayDeathAnimation(animator);
            await Task.Delay(playerScriptableObject.delayAfterDeath * 1000);
            UIService.EndGame(false);
        }

        #region Player Animations
        private void PlayMovementAnimation(Animator animator, bool isRunning) => AnimationService.PlayPlayerMovementAnimation(animator, isRunning);
        public void PlayJumpAnimation(Animator animator) => AnimationService.PlayPlayerJumpAnimation(animator);
        public void PlayTakeDamageAnimation(Animator animator) => AnimationService.PlayPlayerDamageAnimation(animator);
        public void PlayDeathAnimation(Animator animator) => AnimationService.PlayPlayerDeathAnimation(animator);
        public void PlaySlideAnimation(Animator animator) => AnimationService.PlayPlayerSlideAnimation(animator);
        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayPlayerAttackAnimation(animator);
        #endregion
    }
}