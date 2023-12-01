using Platformer.Events;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Cameras{
    public class CameraService{
        #region Service References
        private EventService eventService => GameService.Instance.EventService;
        #endregion

        private CameraController mainCameraController;
        public CameraService(CameraScriptableObject cameraScriptableObject){
            mainCameraController = new CameraController(cameraScriptableObject);
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => eventService.OnVerticalAxisInputReceived.AddListener(mainCameraController.OnVerticalAxisInputReceived);

        #region Camera Effects
        public void ShakeCamera() => mainCameraController.ShakeCamera();
        #endregion
    }
}