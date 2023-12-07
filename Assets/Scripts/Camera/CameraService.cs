using Platformer.Events;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Cameras{
    public class CameraService{

        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        #endregion

        private readonly CameraController mainCameraController;
        public CameraService(CameraScriptableObject cameraScriptableObject){
            mainCameraController = new CameraController(cameraScriptableObject);
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            EventService.OnVerticalAxisInputReceived.AddListener(mainCameraController.OnVerticalAxisInputReceived);
            EventService.OnCameraZoomInputReceived.AddListener(mainCameraController.OnCameraZoomInputReceived);
            EventService.OnPlayerMoved.AddListener(mainCameraController.OnPlayerMoved);
        }

        #region Camera Effects
        public void ShakeCamera() => mainCameraController.ShakeCamera();
        public void BackgroundParallax(Transform[] startPosition) => mainCameraController.BackgroundParallax(startPosition);
        #endregion
    }
}