using UnityEngine;

namespace Platformer.Player{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer characterSprite;
        private Rigidbody2D playerRigidBody;
        public Animator PlayerAnimator => animator;
        public PlayerController Controller { get; private set; }

        public void SetController(PlayerController controllerToSet){
            Controller = controllerToSet;
            InitializeVariables();
        }
        private void InitializeVariables() => playerRigidBody = GetComponent<Rigidbody2D>();

        public void Move(float horizontalInput){
            characterSprite.flipX = horizontalInput < 0;
            float movementSpeed = Controller.GetPlayerMovementSpeed();
            var movementVector = new Vector3(horizontalInput, 0.0f, 0.0f).normalized;
            transform.Translate(movementVector * movementSpeed * Time.deltaTime);
        }
    }
}