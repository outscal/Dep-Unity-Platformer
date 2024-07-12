using System;
using UnityEngine;
using Platformer.Utilities;
using Platformer.Player;
using Platformer.InputSystem;
using Platformer.AnimationSystem;
using Platformer.Level;
using Platformer.UI;
using Platformer.Cameras;

namespace Platformer.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        // Services:
        public PlayerService PlayerService { get; private set; }
        public InputService InputService { get; private set; }
        public AnimationService AnimationService { get; private set; }
        public LevelService LevelService { get; private set; }
        public CameraService CameraService { get; private set; }
        public UIService UIService => uiService;

        // Scriptable Objects:
        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private CameraScriptableObject cameraScriptableObject;
        [SerializeField] private LevelConfiguration levelData;

        //Scene References:
        [SerializeField] private UIService uiService;

        protected override void Awake()
        {
            base.Awake();
            CameraService = new CameraService(cameraScriptableObject);
            LevelService = new LevelService(levelData);
            AnimationService = new AnimationService();
            PlayerService = new PlayerService(playerScriptableObject);
            InputService = new InputService();
        }

        private void Start() 
        {
            UIService.CreateAndShowLevelSelectionUI(levelData.levelConfig.Count);
        }
        
        private void Update()
        {
            InputService.Update();
            PlayerService.Update();
        }

        private void OnDestroy()
        {
            LevelService = null;
            AnimationService = null;
            PlayerService = null;
            InputService = null;
        }
    }
}