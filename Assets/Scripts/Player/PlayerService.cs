using System;
using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.Events;
using Platformer.Game;
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

        public static event Action<GameEndType> OnGameEnd;
        
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

        

        public void MovePlayer(Animator animator, bool isRunning, Vector3 playerPosition){
            PlayMovementAnimation(animator, isRunning);
            EventService.OnPlayerMoved.InvokeEvent(playerPosition);
        }

        public void TakeDamage(int damageToInflict) => playerController.TakeDamage(damageToInflict);

        public async void PlayerDied(Animator animator){ // can be an event 
            UnsubscribeToEvents();
            PlayDeathAnimation(animator);// functionality which can be called in the PlayerDied event
            await Task.Delay(playerScriptableObject.delayAfterDeath * 1000);
            OnGameEnd?.Invoke(GameEndType.LOSE);
        }
        public PlayerController playerController { get; private set; }
        

        private void SpawnPlayer(PlayerScriptableObject playerScriptableObject) => playerController = new PlayerController(playerScriptableObject);

        public void Update() => playerController.Update();
    }
}