using System.Collections;
using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Player
{
    public class PlayerService
    {
        #region Service References
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        #endregion

        private PlayerScriptableObject playerScriptableObject;
        public PlayerController playerController { get; private set; }

        
        public PlayerService(PlayerScriptableObject playerScriptableObject){
            this.playerScriptableObject = playerScriptableObject;
            SpawnPlayer();
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived += playerController.HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived += playerController.HandleTriggerInput;
        }

        private void UnsubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived -= playerController.HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived -= playerController.HandleTriggerInput;
        }
        
        private void SpawnPlayer(){
            playerController = new PlayerController(playerScriptableObject);
        }

        public void MovePlayer(Animator animator, bool isRunning, Vector3 playerPosition){
            PlayMovementAnimation(animator, isRunning);
        }

        public void PlayerDied(Animator animator)
        {
            UnsubscribeToEvents();
            PlayDeathAnimation(animator);
            CoroutineService.StartCoroutine(DelayedRespawnCoroutine(), "PlayerRespawn");
        }

        private IEnumerator DelayedRespawnCoroutine()
        {
            yield return new WaitForSeconds(playerScriptableObject.delayAfterDeath);
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