using UnityEngine;

namespace Platformer.Cameras
{
    public class CameraController 
    {
        private CameraScriptableObject cameraScriptableObject;
        private CameraView cameraView;

        public CameraController(CameraScriptableObject cameraScriptableObject)
        {
            this.cameraScriptableObject = cameraScriptableObject;
            InitializeView();
        }

        private void InitializeView(){
            cameraView = Object.Instantiate(cameraScriptableObject.Prefab);
            cameraView.transform.SetPositionAndRotation(new Vector3(0, 0, -10f), Quaternion.identity);
            cameraView.SetController(this);
        }

        #region Camera Effects
        public void ShakeCamera() => cameraView.ShakeCamera(cameraScriptableObject.shakeMagnitude, cameraScriptableObject.shakeDuration);
        public void OnVerticalAxisInputReceived(float verticalInput){
            // look up or down depending on the input
            var movementDirection = new Vector3(0, verticalInput, 0).normalized;
            if(movementDirection != Vector3.zero)
                Debug.Log("look up or down depending on the input");    
        }
        #endregion
    }
}