using System.Threading.Tasks;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Cameras{
    [RequireComponent(typeof(Camera))]
    public class CameraView : MonoBehaviour
    {
        private CameraController cameraController;
        private Camera cameraComponent;

        private Transform playerTransform => GameService.Instance.PlayerService.PlayerController.PlayerView.transform;

        private Vector3 originalPosition;

        public void SetController(CameraController cameraController){
            this.cameraController = cameraController;
            InitilizeVariables();
        }

        private void InitilizeVariables() => cameraComponent = GetComponent<Camera>();

        #region Camera Shake Effect
        public void ShakeCamera(float magnitude, float duration){
            originalPosition = transform.localPosition;
            Shake(magnitude, duration);
        }

        private async void Shake(float magnitude, float duration)
        {
            float elapsed = 0f;

            while (elapsed < duration)
            {
                Vector3 randomPos = originalPosition + Random.insideUnitSphere * magnitude;
                transform.localPosition = randomPos;

                elapsed += Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime * 1000));
            }

            transform.localPosition = originalPosition;
        }

        #endregion

        #region Camera Zoom
        public void Zoom(bool isZoomIn, float sizeIncrement, float minSize, float maxSize){
            cameraComponent.orthographicSize += isZoomIn ? -sizeIncrement : sizeIncrement;
            cameraComponent.orthographicSize = Mathf.Clamp(cameraComponent.orthographicSize, minSize, maxSize);
        }
        #endregion

        #region Follow Player
        public void FollowPlayer(Vector3 playerPosition) => transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        #endregion

        #region Background Parallax Effect
        public void ParallaxEffect(Transform sprite){
            var zPosition = sprite.position.z;
            Vector3 deltaMovement = transform.position - originalPosition;
            var distanceFromPlayer = sprite.position.z - playerTransform.position.z;
            var clippingPlane = transform.position.z + (distanceFromPlayer > 0 ? cameraComponent.farClipPlane : cameraComponent.nearClipPlane);
            var parallaxFactor = Mathf.Abs(distanceFromPlayer) / clippingPlane;
            sprite.position += deltaMovement * parallaxFactor;
            sprite.position = new Vector3(sprite.position.x, sprite.position.y, zPosition);
            originalPosition = transform.position;
        }
        #endregion
    }   
}