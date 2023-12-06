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
    }
}