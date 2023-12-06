using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.Events;
using Platformer.Main;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Player
{
    public class PlayerService
    {
        #region Service References
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        private EventService EventService => GameService.Instance.EventService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerController playerController { get; private set; }

        
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SpawnPlayer();
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            EventService.OnHorizontalAxisInputReceived.AddListener(playerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.AddListener(playerController.HandleTriggerInput);
        }

        private void UnsubscribeToEvents(){
            EventService.OnHorizontalAxisInputReceived.RemoveListener(playerController.HandleHorizontalMovementAxisInput);
            EventService.OnPlayerTriggerInputReceived.RemoveListener(playerController.HandleTriggerInput);
        }
        
        private void SpawnPlayer(){
            playerController = new PlayerController(playerScriptableObject);
        }

        public void MovePlayer(Animator animator, bool isRunning, Vector3 playerPosition){
            PlayMovementAnimation(animator, isRunning);
            EventService.OnPlayerMoved.InvokeEvent(playerPosition);
        }

        public async void Die(){
            UnsubscribeToEvents();
            playerController.Die();
            await Task.Delay(2000);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        #region Player Animations
        private void PlayMovementAnimation(Animator animator, bool isRunning) => AnimationService.PlayPlayerMovementAnimation(animator, isRunning);
        public void PlayJumpAnimation(Animator animator) => AnimationService.PlayPlayerJumpAnimation(animator);
        public void PlayDamageAnimation(Animator animator) => AnimationService.PlayPlayerDamageAnimation(animator);
        public void PlayDeathAnimation(Animator animator) => AnimationService.PlayPlayerDeathAnimation(animator);
        public void PlaySlideAnimation(Animator animator) => AnimationService.PlayPlayerSlideAnimation(animator);
        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayPlayerAttackAnimation(animator);
        #endregion
    }
}