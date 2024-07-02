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
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        public PlayerController playerController { get; private set; }
        private float delayAfterDeath;
        
        public PlayerService(PlayerScriptableObject playerScriptableObject)
        {
            intializeVariables(playerScriptableObject);
            SpawnPlayer(playerScriptableObject);
            SubscribeToEvents();
        }

        void intializeVariables(PlayerScriptableObject playerScriptableObject) =>
            delayAfterDeath = playerScriptableObject.delayAfterDeath;
        
        private void SpawnPlayer(PlayerScriptableObject playerScriptableObject) => 
            playerController = new PlayerController(playerScriptableObject);

        // TODO: These events should be subscribed and unsubscribed by PlayerController directly.
        private void SubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived += playerController.HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived += playerController.HandleTriggerInput;
        }

        private void UnsubscribeToEvents(){
            InputService.OnHorizontalAxisInputReceived -= playerController.HandleHorizontalMovementAxisInput;
            InputService.OnPlayerTriggerInputReceived -= playerController.HandleTriggerInput;
        }

        // TODO: This function will not be needed in this class. PlayerController will directly communicate with Animation Service for handling Player Animations.
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

        // TODO: All these methods will not be needed as Generic methods have been created inside Animation Service in previous branch. Use them.
        // TODO: All this Player Animation should not be in PlayerService as it is too specific to be handled by PlayerService.
        // TODO: Either move this to PlayerController or create a PlayerAnimatorController.cs below PlayerController if PlayerController is getting too cluttered.
        private void PlayMovementAnimation(Animator animator, bool isRunning) => AnimationService.PlayPlayerMovementAnimation(animator, isRunning);
        public void PlayJumpAnimation(Animator animator) => AnimationService.PlayPlayerJumpAnimation(animator);
        public void PlayDamageAnimation(Animator animator) => AnimationService.PlayPlayerDamageAnimation(animator);
        public void PlayDeathAnimation(Animator animator) => AnimationService.PlayPlayerDeathAnimation(animator);
        public void PlaySlideAnimation(Animator animator) => AnimationService.PlayPlayerSlideAnimation(animator);
        public void PlayAttackAnimation(Animator animator) => AnimationService.PlayPlayerAttackAnimation(animator);
    }
}