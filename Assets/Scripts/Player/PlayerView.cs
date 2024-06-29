using System.Threading.Tasks;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Player{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class PlayerView : MonoBehaviour, IDamagable
    {
        public PlayerController Controller { get; private set; }

        #region Editor properties
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer characterSprite;
        [SerializeField] private LayerMask groundLayer;
        #endregion

        #region Private variables
        private BoxCollider2D playerBoxCollider;
        private Rigidbody2D playerRigidBody;
        #endregion

        #region Getters
        public Animator PlayerAnimator => animator;
        public LayerMask GroundLayer => groundLayer;
        [HideInInspector] public Vector3 Position => transform.position;
        #endregion

        public void SetController(PlayerController controllerToSet){
            Controller = controllerToSet;
            InitializeVariables();
        }

        private void InitializeVariables(){
            playerRigidBody = GetComponent<Rigidbody2D>();
            playerBoxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update() => Controller?.Update();

        #region Movement Functions
        public void SetCharacterSpriteDirection(bool flipX) => characterSprite.flipX = flipX;

        public void TranslatePlayer(Vector3 translateVector) => transform.Translate(translateVector);
        #endregion

        #region Take Damage Function
        public void TakeDamage(int damage) => Controller.TakeDamage(damage);
        #endregion

        public void SetVelocity(Vector2 newVelocity) => playerRigidBody.velocity = newVelocity; 
        public void AddVelocity(Vector2 additionalVelocity) => playerRigidBody.velocity += additionalVelocity; 
        public Vector2 GetVelocity() => playerRigidBody.velocity;
        public void AddForce(Vector2 forceToAdd, ForceMode2D forceMode2D) => playerRigidBody.AddForce(forceToAdd, forceMode2D);
        public void SetPositionAndRotation(Vector3 position, Quaternion rotation) => transform.SetLocalPositionAndRotation(position, rotation);
        public Bounds GetColliderBounds() => playerBoxCollider.bounds;
    }
}