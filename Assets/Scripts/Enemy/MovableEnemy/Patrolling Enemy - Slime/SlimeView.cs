using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SlimeView : EnemyView, IDamagable
    {
        private SlimeController SlimeController => Controller as SlimeController;

        private BoxCollider2D boxCollider;
        [SerializeField] private LayerMask groundLayer;

        #region Properties
        [HideInInspector] public bool IsGrounded // not being used, since there is no jumping enemy
        {
            get {
                var offset = 0.3f;
                RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + offset, groundLayer);
                return raycastHit.collider != null;
            }
            private set => IsGrounded = value;
        }

        [HideInInspector] public bool WallCheck{
            get{
                var offset = 0.3f;
                var raycastHit = Physics2D.Raycast(boxCollider.bounds.center, SlimeController.IsMovingRight ? Vector2.right : Vector2.left, boxCollider.bounds.extents.x + offset, groundLayer);
                return raycastHit.collider != null;
            }
            private set => WallCheck = value;
        }
        #endregion

        private void Start() => boxCollider = GetComponent<BoxCollider2D>();

     
        #region Patrol Functionality
        // public void Move(Vector3 position, float speed, bool isMovingRight){
        //     SetCharacterSpriteDirection(isMovingRight);
        //     transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        // }
        //
        // private void SetCharacterSpriteDirection(bool isMovingRight) {
        //     if (isMovingRight != (transform.localScale.x < 0))
        //         transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        // }
        #endregion

        #region IDamagable Implementation
        public void TakeDamage(int damageToInflict) => Controller.TakeDamage(damageToInflict);
        #endregion
    }
}