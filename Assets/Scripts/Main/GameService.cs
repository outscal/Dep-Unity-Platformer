/**  This script demonstrates implementation of the Service Locator Pattern.
*  If you're interested in learning about Service Locator Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/

using System;
using UnityEngine;
using Platformer.Utilities;
using Platformer.Player;
using Platformer.InputSystem;
using Platformer.AnimationSystem;
using Platformer.Level;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Platformer.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        public PlayerService PlayerService { get; private set; }
        public InputService InputService { get; private set; }
        public AnimationService AnimationService { get; private set; }
        public LevelService LevelService { get; private set; }
        
        // Scriptable Objects:
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [FormerlySerializedAs("levelScriptableObjects")] [SerializeField] private List<LevelScriptableObject> levelConfigurations;
        
        //Variables
        private Animator playerAnimator;

        protected override void Awake()
        {
            base.Awake();
            LevelService = new LevelService(levelConfigurations);
            PlayerService = new PlayerService(playerScriptableObject);
            InputService = new InputService();
            playerAnimator = PlayerService.playerController.PlayerView.PlayerAnimator;
            
        }

        private void Start()
        {
            AnimationService = new AnimationService(playerAnimator);
        }

        private void Update()
        {
            InputService.UpdateInputService();
            PlayerService.Update();
        }
        
    }
}