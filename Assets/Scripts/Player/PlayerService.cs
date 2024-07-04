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

        
        public PlayerService(PlayerScriptableObject playerScriptableObject)
        {
            SpawnPlayer(playerScriptableObject);
        }

        private void SpawnPlayer(PlayerScriptableObject playerScriptableObject) => 
            playerController = new PlayerController(playerScriptableObject);

        public void Update()
        {
            playerController.Update();
        }
    }
}