using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy{
    [RequireComponent(typeof(Animator))]
    public class MushroomHeadView : EnemyView, IDamagable
    {
        [HideInInspector]public Animator Animator;

        #region Attack variabes
        public Transform MeleeContainer;
        #endregion

        private BoxCollider2D boxCollider;
        [SerializeField] private LayerMask groundLayer;

        #region Properties
        [HideInInspector] public bool IsGrounded
        {
            get {
                var offset = 0.3f;
                RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Vector2.down, boxCollider.bounds.extents.y + offset, groundLayer);
                return raycastHit.collider != null;
            }
            private set => IsGrounded = value;
        }
        #endregion

        public override void SetController(EnemyController controllerToSet)
        {
            base.SetController(controllerToSet);
            Animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
        }

        #region Patrol Functionality
        public void Move(Vector3 position, float speed, bool isMovingRight){
            SetCharacterSpriteDirection(isMovingRight);
            transform.position = Vector3.MoveTowards(transform.position, position, speed * Time.deltaTime);
        }

        private void SetCharacterSpriteDirection(bool isMovingRight) {
            if (isMovingRight != (transform.localScale.x > 0))
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        #endregion

        private void Update() => (Controller as MushroomHeadController).Update();     

        #region IDamagable Implementation
        public void TakeDamage(int damageToInflict) => Controller.TakeDamage(damageToInflict);
        #endregion
    }
}