using System.Collections;
using UnityEngine;

namespace Platformer.Cameras{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        private CameraController cameraController;

        private Vector3 originalPosition;

        public void SetController(CameraController cameraController) => this.cameraController = cameraController;
    }   
}