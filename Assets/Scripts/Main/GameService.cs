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
using Platformer.Level;
using System.Collections.Generic;
using Platformer.UI;
using Platformer.Enemy;
using Platformer.Cameras;

namespace Platformer.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        #region Services
        public EventService EventService { get; private set; }
        public PlayerService PlayerService { get; private set; }
        public InputService InputService { get; private set; }
        public AnimationService AnimationService { get; private set; }
        public LevelService LevelService { get; private set; }
        public EnemyService EnemyService { get; private set; }
        public CameraService CameraService { get; private set; }
        public UIService UIService => uiService;
        #endregion

        #region ScriptableObjestsReferences
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private List<LevelScriptableObject> levelScriptableObjects;
        [SerializeField] private CameraScriptableObject cameraScriptableObject;
        #endregion

        #region Scene Refrences
        [SerializeField] private UIService uiService;
        #endregion

        protected override void Awake()
        {
            base.Awake();
            EventService = new EventService();
            CameraService = new CameraService(cameraScriptableObject);
            LevelService = new LevelService(levelScriptableObjects);
            EnemyService = new EnemyService();
            PlayerService = new PlayerService(playerScriptableObject);
            InputService = new InputService();
            AnimationService = new AnimationService();
        }

        private void Update()
        {
            InputService.UpdateInputService();
            EnemyService.Update();
        }

        private void Start() => UIService.ShowLevelSelectionUI(levelScriptableObjects.Count);
    }
}