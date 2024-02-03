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
        [SerializeField] private Transform meleeContainer;
        public Transform MeleeContainer => meleeContainer;
        [SerializeField] private LayerMask groundLayer;
        #endregion

        #region Private variables
        private BoxCollider2D playerBoxCollider;
        private Rigidbody2D playerRigidBody;
        private float translateSpeed = 0;
        #endregion

        #region Getters
        public Animator PlayerAnimator => animator;
        [HideInInspector] public Vector3 Position => transform.position;
        #endregion

        #region Properties
        [HideInInspector] public bool IsGrounded // grounded check is performed using Raycast
        {
            get {
                var offset = 0.3f;
                RaycastHit2D raycastHit = Physics2D.Raycast(playerBoxCollider.bounds.center, Vector2.down, playerBoxCollider.bounds.extents.y + offset, groundLayer);
                return raycastHit.collider != null;
            }
            private set => IsGrounded = value;
        }
        [HideInInspector] public PlayerStates PlayerState { get; private set; }
        #endregion

        public void SetController(PlayerController controllerToSet){
            Controller = controllerToSet;
            InitializeVariables();
        }

        private void InitializeVariables(){
            playerRigidBody = GetComponent<Rigidbody2D>();
            playerBoxCollider = GetComponent<BoxCollider2D>();
        }

        private void Update(){
            var check = IsGrounded;
            if(playerRigidBody.velocity.y < 0){
                playerRigidBody.velocity += (Controller.GetFallMultiplier() - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }else if(playerRigidBody.velocity.y > 0){
                playerRigidBody.velocity += (Controller.GetLowJumpMultiplier() - 1) * Physics2D.gravity.y * Time.deltaTime * Vector2.up;
            }
        }

        #region Movement Functions
        public void Move(float horizontalInput, float playerMovementSpeed){
            UpdateRunningStatus(horizontalInput);
            if(horizontalInput != 0)
                SetCharacterSpriteDirection(horizontalInput < 0);
            if (PlayerState != PlayerStates.SLIDE) translateSpeed = playerMovementSpeed;
            TranslatePlayer(horizontalInput);
        }

        private void SetCharacterSpriteDirection(bool flipX) => transform.localScale = new Vector3(flipX ? -1 : 1, 1, 1);
        private void UpdateRunningStatus(float horizontalInput) => PlayerState = horizontalInput != 0 ? PlayerStates.RUNNING : PlayerStates.IDLE;
        private void TranslatePlayer(float horizontalInput){
            var movementVector = new Vector3(horizontalInput, 0.0f, 0.0f).normalized;
            transform.Translate(translateSpeed * Time.deltaTime * movementVector);
        }
        #endregion

        #region JUMP
        public bool CanJump() => IsGrounded && (PlayerState == PlayerStates.IDLE || PlayerState == PlayerStates.RUNNING);

        // public void Jump(float jumpForce) => playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        public void Jump(float jumpForce) => playerRigidBody.velocity = Vector2.up * jumpForce;

        // private void Jump() // direct changing the position through translate method
        // {
        //     var force = Controller.GetJumpForce();
        //     float jumpHeight = force * Time.deltaTime; // Convert force to a height
        //     transform.Translate(Vector2.up * jumpHeight);
        // }
        #endregion

        #region SLIDE
        public bool CanSlide() => IsGrounded && PlayerState == PlayerStates.RUNNING;

        public async void Slide(float slidingSpeed, float slidingTime)
        {
            var temp = translateSpeed;
            SetSlidingState(slidingSpeed, true);
            await Task.Delay((int)(slidingTime * 1000));
            SetSlidingState(temp, false);
        }

        private void SetSlidingState(float speed, bool isSliding)
        {
            translateSpeed = speed;
            PlayerState = isSliding ? PlayerStates.SLIDE : PlayerStates.IDLE;
        }

        #endregion

        #region ATTACK
        public bool CanAttack() => IsGrounded && (PlayerState == PlayerStates.IDLE || PlayerState == PlayerStates.RUNNING);

        public void Attack() => PlayerState = PlayerStates.ATTACK;

        #endregion

        #region Take Damage Function
        public void TakeDamage(int damage) => Controller.TakeDamage(damage);
        #endregion
    }
}