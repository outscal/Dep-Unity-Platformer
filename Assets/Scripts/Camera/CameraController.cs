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
    }
}