using System.Threading.Tasks;
using Platformer.AnimationSystem;
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
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerController playerController { get; private set; }

        
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => EventService.OnLevelSelected.AddListener(SpawnPlayer);

        private void UnsubscribeToEvents(){
            EventService.OnLevelSelected.RemoveListener(SpawnPlayer);
            EventService.OnHorizontalAxisInputReceived.RemoveListener(playerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.RemoveListener(playerController.HandleTriggerInput);
        }

        private void SpawnPlayer(int levelId){
            playerController = new PlayerController(playerScriptableObject);
            EventService.OnHorizontalAxisInputReceived.AddListener(playerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.AddListener(playerController.HandleTriggerInput);
        }

        public void MovePlayer(Animator animator, bool isRunning, Vector3 playerPosition){
            PlayMovementAnimation(animator, isRunning);
            EventService.OnPlayerMoved.InvokeEvent(playerPosition);
        }

        public void TakeDamage(int damageToInflict) => playerController.TakeDamage(damageToInflict);

        public async void PlayerDied(Animator animator){ // can be an event 
            UnsubscribeToEvents();
            PlayDeathAnimation(animator); // functionality which can be called in the PlayerDied event
            await Task.Delay(playerScriptableObject.delayAfterDeath * 1000);
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.GAME_LOST);
            UIService.EndGame(false); // functionality which can be called in the PlayerDied event
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