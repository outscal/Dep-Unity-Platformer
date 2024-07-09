using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerView : MonoBehaviour
    {
        private Animator animator;
        private SpriteRenderer characterSprite;
        [SerializeField] private LayerMask groundLayer;

        private PlayerController controller;
        private BoxCollider2D playerBoxCollider;
        private Rigidbody2D playerRigidBody;

        //Getters
        public Animator PlayerAnimator => animator;
        public LayerMask GroundLayer => groundLayer;
        public Vector3 Position => transform.position;

        public void SetController(PlayerController controllerToSet)
        {
            controller = controllerToSet;
            InitializeVariables();
        }

        private void InitializeVariables()
        {
            playerRigidBody = GetComponent<Rigidbody2D>();
            playerBoxCollider = GetComponent<BoxCollider2D>();
            animator = GetComponent<Animator>();
            characterSprite = GetComponent<SpriteRenderer>();
        }
        
        public void TakeDamage(int damage) => controller.TakeDamage(damage);
        public void TranslatePlayer(Vector3 translateVector) => transform.Translate(translateVector);
        public void AddVelocity(Vector2 additionalVelocity) => playerRigidBody.velocity += additionalVelocity; 
        public void AddForce(Vector2 forceToAdd, ForceMode2D forceMode2D) => playerRigidBody.AddForce(forceToAdd, forceMode2D);
        public void SetCharacterSpriteDirection(bool flipX) => characterSprite.flipX = flipX;
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => transform.SetLocalPositionAndRotation(position, rotation);
        public void SetVelocity(Vector2 newVelocity) => playerRigidBody.velocity = newVelocity; 
        public Vector2 GetVelocity() => playerRigidBody.velocity;
        public Bounds GetColliderBounds() => playerBoxCollider.bounds;
    }
}