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
        [SerializeField] private Transform meleeContainer;
        public Transform MeleeContainer => meleeContainer;
        [SerializeField] private LayerMask groundLayer;
        #endregion

        #region Private variables
        private BoxCollider2D playerBoxCollider;
        private Rigidbody2D playerRigidBody;
        #endregion

        #region Getters
        public Animator PlayerAnimator => animator;
        public LayerMask GroundLayer => groundLayer;
        public BoxCollider2D PlayerBoxCollider => playerBoxCollider;
        public Rigidbody2D PlayerRigidBody => playerRigidBody;
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
        public void SetCharacterSpriteDirection(bool flipX) => transform.localScale = new Vector3(flipX ? -1 : 1, 1, 1);

        public void TranslatePlayer(Vector3 translateVector) => transform.Translate(translateVector);
        #endregion

        #region JUMP
        // public void Jump(float jumpForce) => playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        public void Jump(float jumpForce) => playerRigidBody.velocity = Vector2.up * jumpForce;

        // private void Jump() // direct changing the position through translate method
        // {
        //     var force = Controller.GetJumpForce();
        //     float jumpHeight = force * Time.deltaTime; // Convert force to a height
        //     transform.Translate(Vector2.up * jumpHeight);
        // }
        #endregion

        #region Take Damage Function
        public void TakeDamage(int damage) => Controller.TakeDamage(damage);
        #endregion

        public void CollectCoin(int coinValue) => Controller?.CollectCoin(coinValue);

        public void CollectLevelKey() => Controller?.CollectLevelKey();
    }
}