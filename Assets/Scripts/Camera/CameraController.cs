using UnityEngine;

namespace Platformer.Cameras
{
    public class CameraController 
    {
        private readonly CameraScriptableObject cameraScriptableObject;
        public CameraView CameraView { get; private set; }

        public CameraController(CameraScriptableObject cameraScriptableObject)
        {
            this.cameraScriptableObject = cameraScriptableObject;
            InitializeView();
        }

        private void InitializeView(){
            CameraView = Object.Instantiate(cameraScriptableObject.Prefab);
            CameraView.transform.SetPositionAndRotation(new Vector3(0, 0, -10f), Quaternion.identity);
            CameraView.SetController(this);
        }

        public void SetCameraBounds(CameraBounds cameraBounds) => CameraView.SetCameraBounds(cameraBounds);

        #region Camera Effects
        public void ShakeCamera() => CameraView.ShakeCamera(cameraScriptableObject.shakeMagnitude, cameraScriptableObject.shakeDuration);

        public void OnVerticalAxisInputReceived(float verticalInput){
            // look up or down depending on the input
            var movementDirection = new Vector3(0, verticalInput, 0).normalized;
            if(movementDirection != Vector3.zero)
                Debug.Log("look up or down depending on the input");    
        }

        public void OnCameraZoomInputReceived(ZoomType zoomType) => CameraView.Zoom(zoomType, cameraScriptableObject.cameraSizeIncrement, cameraScriptableObject.minSize, cameraScriptableObject.maxSize);

        public void OnPlayerMoved(Vector3 playerPosition) => CameraView.PlayerMoved(playerPosition);

        public void BackgroundParallax(Transform[] sprites){
            foreach(var sprite in sprites){
                CameraView.ParallaxEffect(sprite);
            }
        }
        #endregion
    }
}