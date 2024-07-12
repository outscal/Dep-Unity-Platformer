using Platformer.Game;
using Platformer.InputSystem;
using Platformer.Level;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Cameras{
    public class CameraService{

        #region Service References
        #endregion

        private CameraController mainCameraController;
        public CameraService(CameraScriptableObject cameraScriptableObject){
            mainCameraController = new CameraController(cameraScriptableObject);
            SubscribeToEvents();
        }

        private void SubscribeToEvents()
        {
            InputService.OnVerticalAxisInputReceived += mainCameraController.OnVerticalAxisInputReceived;
            InputService.OnCameraZoomInputReceived += mainCameraController.OnCameraZoomInputReceived;
            PlayerService.OnPlayerMoved += (mainCameraController.OnPlayerMoved);
            LevelService.OnLevelEnd += LevelEnd;
        }
        private void UnsubscribeToEvents()
        {
            InputService.OnVerticalAxisInputReceived -= mainCameraController.OnVerticalAxisInputReceived;
            InputService.OnCameraZoomInputReceived -= mainCameraController.OnCameraZoomInputReceived;
            PlayerService.OnPlayerMoved -= (mainCameraController.OnPlayerMoved);
            LevelService.OnLevelEnd -= LevelEnd;
        }

        public void SetCameraBounds(CameraBounds cameraBounds) => mainCameraController.SetCameraBounds(cameraBounds);

        #region Camera Effects
        public void ShakeCamera() => mainCameraController.ShakeCamera();
        public void BackgroundParallax(Transform[] startPosition) => mainCameraController.BackgroundParallax(startPosition);
        #endregion
    
        private void LevelEnd()
        {
            UnsubscribeToEvents();
            mainCameraController = null;
        }
    }
}