using Platformer.Cameras;
using Platformer.Main;
using Platformer.Player;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    #region Service References
    private CameraService CameraService => GameService.Instance.CameraService;
    private PlayerService PlayerService => GameService.Instance.PlayerService;
    #endregion 

    private Transform cameraTransform;
    private Transform playerTransform;
    private Vector2 startPosition;
    private float zPosition;
    private Vector2 Travel => (Vector2)cameraTransform.position - startPosition;
    private float DistanceFromPlayer => transform.position.z - playerTransform.position.z;
    private float ClippingPlane => cameraTransform.position.z + (DistanceFromPlayer > 0 ? cameraTransform.GetComponent<Camera>().farClipPlane : cameraTransform.GetComponent<Camera>().nearClipPlane);
    private float ParallaxFactor => Mathf.Abs(DistanceFromPlayer) / ClippingPlane;

    private void Start(){
        cameraTransform = CameraService.mainCameraController.CameraView.transform;
        playerTransform = PlayerService.playerController.playerView.transform;
        startPosition = transform.position;
        zPosition = transform.position.z;
    }


    private void LateUpdate(){
        var newPos = startPosition + Travel * ParallaxFactor;
        transform.position = new Vector3(newPos.x, newPos.y, zPosition);
    }
}
