/**  This script demonstrates implementation of the Service Locator Pattern.
*  If you're interested in learning about Service Locator Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/

using UnityEngine;
using Platformer.Utilities;
using Platformer.Player;
using Platformer.InputSystem;
using Platformer.AnimationSystem;
using Platformer.Level;
using System.Collections.Generic;

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
        // TODO: Create A single SO called LevelConfigurations which has a list of LevelSO. This decreases inspector dependency.
        [SerializeField] private List<LevelScriptableObject> levelScriptableObjects;  

        protected override void Awake()
        {
            base.Awake();
            LevelService = new LevelService(levelScriptableObjects);
            PlayerService = new PlayerService(playerScriptableObject);
            InputService = new InputService();
            AnimationService = new AnimationService();
        }

        private void Update() => InputService.UpdateInputService();
    }
}