using UnityEngine;

namespace Platformer.Cameras
{
    public class CameraController 
    {
        private CameraScriptableObject cameraScriptableObject;
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

        #region Camera Effects
        public void ShakeCamera() => CameraView.ShakeCamera(cameraScriptableObject.shakeMagnitude, cameraScriptableObject.shakeDuration);
        public void OnVerticalAxisInputReceived(float verticalInput){
            // look up or down depending on the input
            var movementDirection = new Vector3(0, verticalInput, 0).normalized;    
        }

        public void OnPlayerMoved(Vector3 playerPosition) => CameraView.FollowPlayer(playerPosition);
        #endregion
    }
}