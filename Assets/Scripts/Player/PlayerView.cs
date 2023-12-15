using System.Threading.Tasks;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Player{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerView : MonoBehaviour, ICustomGravity
    {
        [SerializeField] private Animator animator;
        [SerializeField] private Transform groundCheckPoint; // at the feet of the player
        [SerializeField] private Vector2 groundCheckSize = new(0.49f, 0.03f);
        [SerializeField] private Transform meleeContainer;
        private bool isFacingRight = true;
        private Rigidbody2D playerRigidBody;
        public Animator PlayerAnimator => animator;
        public Transform MeleeContainer => meleeContainer;
        public PlayerController Controller { get; private set; }
        public LayerMask groundLayer;

        [HideInInspector] public Vector3 Position => transform.position;
        [HideInInspector] public bool IsGrounded
        {
            get => Physics2D.OverlapBox(groundCheckPoint.position, groundCheckSize, 0, groundLayer);
            private set => IsGrounded = value;
        }
        [HideInInspector] public bool IsRunning { get; private set; }
        [HideInInspector] public bool IsSliding { get; private set; }

        private float translateSpeed = 0;

        public void SetController(PlayerController controllerToSet){
            Controller = controllerToSet;
            InitializeVariables();
        }
        private void InitializeVariables() => playerRigidBody = GetComponent<Rigidbody2D>();

        public void Move(float horizontalInput, float playerMovementSpeed){
            if(horizontalInput != 0) IsRunning = true;
            else IsRunning = false;
            if ((horizontalInput > 0 && !isFacingRight) || (horizontalInput < 0 && isFacingRight))
            {
                Flip();
            }
            if(!IsSliding)
                translateSpeed = playerMovementSpeed;
            var movementVector = new Vector3(horizontalInput, 0.0f, 0.0f).normalized;
            transform.Translate(translateSpeed * Time.deltaTime * movementVector);
        }

        private void Flip()
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }

        private void Update(){
            if(!IsGrounded)
                ApplyGravity();
        }

        #region JUMP
        public bool CanJump() => IsGrounded;

        public void Jump(float jumpForce) => playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        // private void Jump() // velocity change method for jump
        // {
        //     var force = Controller.GetJumpForce();
        //     playerRigidBody.velocity = new Vector2(playerRigidBody.velocity.x, force);
        // }
        // private void Jump() // direct changing the position through translate method
        // {
        //     var force = Controller.GetJumpForce();
        //     float jumpHeight = force * Time.deltaTime; // Convert force to a height
        //     transform.Translate(Vector2.up * jumpHeight);
        // }
        #endregion

        #region SLIDE
        public bool CanSlide() => IsGrounded && IsRunning && !IsSliding;

        public async void Slide(float slidingSpeed, float slidingTime){
            var temp = translateSpeed;
            translateSpeed = slidingSpeed;
            IsSliding = true;
            await Task.Delay((int)(slidingTime * 1000));
            translateSpeed = temp;
            IsSliding = false;
        }
        #endregion

        #region GRAVITY
        public void ApplyGravity() => playerRigidBody.AddForce(Vector2.down * 0.09f, ForceMode2D.Impulse);
        #endregion

        public void CollectCoin(int coinValue) => Controller?.CollectCoin(coinValue);
    }
}