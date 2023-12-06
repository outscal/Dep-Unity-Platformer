using Platformer.Events;
using Platformer.Main;

namespace Platformer.Cameras{
    public class CameraService{
        #region Service References
        private EventService EventService => GameService.Instance.EventService;
        #endregion

        public readonly CameraController mainCameraController;
        public CameraService(CameraScriptableObject cameraScriptableObject){
            mainCameraController = new CameraController(cameraScriptableObject);
            SubscribeToEvents();
        }

        private void SubscribeToEvents(){
            EventService.OnVerticalAxisInputReceived.AddListener(mainCameraController.OnVerticalAxisInputReceived);
            EventService.OnPlayerMoved.AddListener(mainCameraController.OnPlayerMoved);
        }

        #region Camera Effects
        public void ShakeCamera() => mainCameraController.ShakeCamera();
        #endregion
    }
}