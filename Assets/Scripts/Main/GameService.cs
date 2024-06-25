/**  This script demonstrates implementation of the Service Locator Pattern.
*  If you're interested in learning about Service Locator Pattern, 
*  you can find a dedicated course on Outscal's website.
*  Link: https://outscal.com/courses
**/

using UnityEngine;
using Platformer.Utilities;
using Platformer.Events;
using Platformer.Player;
using Platformer.InputSystem;
using Platformer.AnimationSystem;

namespace Platformer.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        #region Services
        public EventService EventService { get; private set; }
        public PlayerService PlayerService { get; private set; }
        public InputService InputService { get; private set; }
        public AnimationService AnimationService { get; private set; }
        #endregion

        #region ScriptableObjestsReferences
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            EventService = new EventService();
            PlayerService = new PlayerService(playerScriptableObject);
            InputService = new InputService();
            AnimationService = new AnimationService();
        }

        private void Update() => InputService.UpdateInputService();
    }
}