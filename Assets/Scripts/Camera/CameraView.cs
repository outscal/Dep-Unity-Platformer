using System.Collections;
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
        public void Zoom(ZoomType zoomType, float sizeIncrement, float minSize, float maxSize)
        {
            float newSize = Mathf.Clamp(cameraComponent.orthographicSize + (zoomType == ZoomType.ZOOMIN ? -sizeIncrement : sizeIncrement), minSize, maxSize);
            cameraComponent.orthographicSize = newSize;
        }

        #endregion

        #region Follow Player
        public void FollowPlayer(Vector3 playerPosition) => transform.position = new Vector3(playerPosition.x, playerPosition.y, transform.position.z);
        #endregion

        #region Background Parallax Effect
        public void ParallaxEffect(Transform sprite)
        {
            float parallaxFactor = CalculateParallaxFactor(sprite);
            Vector3 newPosition = CalculateNewSpritePosition(sprite, parallaxFactor);
            sprite.position = newPosition;
            originalPosition = transform.position;
        }

        private float CalculateParallaxFactor(Transform sprite)
        {
            float distanceFromPlayer = sprite.position.z - playerTransform.position.z;
            float clippingPlane = transform.position.z + (distanceFromPlayer > 0 ? cameraComponent.farClipPlane : cameraComponent.nearClipPlane);
            return Mathf.Abs(distanceFromPlayer) / clippingPlane;
        }

        private Vector3 CalculateNewSpritePosition(Transform sprite, float parallaxFactor)
        {
            Vector3 deltaMovement = transform.position - originalPosition;
            Vector3 newPosition = sprite.position + deltaMovement * parallaxFactor;
            return new Vector3(newPosition.x, newPosition.y, sprite.position.z);
        }
        #endregion
    }   
}