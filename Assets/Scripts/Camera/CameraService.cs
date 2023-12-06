using Platformer.Events;
using Platformer.Main;

namespace Platformer.Cameras{
    public class CameraService{
        private CameraController mainCameraController;
        public CameraService(CameraScriptableObject cameraScriptableObject) => mainCameraController = new CameraController(cameraScriptableObject);
    }
}