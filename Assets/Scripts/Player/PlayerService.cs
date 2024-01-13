using Platformer.AnimationSystem;
using Platformer.Events;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Player
{
    public class PlayerService
    {
        #region Service References
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        private EventService EventService => GameService.Instance.EventService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        private PlayerController playerController;

        
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SpawnPlayer();
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            EventService.OnHorizontalAxisInputReceived.AddListener(playerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.AddListener(playerController.HandleTriggerInput);
        }
        
        private void SpawnPlayer(){
            playerController = new PlayerController(playerScriptableObject);
        }

        #region Player Animations
        public void PlayMovementAnimation(Animator animator, bool isRunning) => AnimationService.PlayPlayerMovementAnimation(animator, isRunning);
        public void PlayJumpAnimation(Animator animator) => AnimationService.PlayPlayerJumpAnimation(animator);
        public void PlayDamageAnimation(Animator animator) => AnimationService.PlayPlayerDamageAnimation(animator);
        public void PlayDeathAnimation(Animator animator) => AnimationService.PlayPlayerDeathAnimation(animator);
        public void PlaySlideAnimation(Animator animator) => AnimationService.PlayPlayerSlideAnimation(animator);
        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayPlayerAttackAnimation(animator);
        #endregion
    }
}