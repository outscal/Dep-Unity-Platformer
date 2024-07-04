using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; private set; }

        public virtual void SetController(EnemyController controllerToSet) => Controller = controllerToSet;
        public Vector3 GetPosition() => transform.position;
        public void SetPosition(Vector3 positionToSet) => transform.position = positionToSet; 
        public Vector3 GetLocalScale() => transform.localScale;

        public void SetLocalScale(Vector3 localScale) => transform.localScale = localScale;
        
        private BoxCollider2D boxCollider;
        
        
        [HideInInspector] public bool WallCheck{
            get{
                var offset = 0.3f;
                var raycastHit = Physics2D.Raycast(boxCollider.bounds.center, Controller.MovableEnemyController.IsMovingRight ? Vector2.right : Vector2.left, boxCollider.bounds.extents.x + offset);
                return raycastHit.collider != null;
            }
            private set => WallCheck = value;
        }

        private void OnTriggerEnter2D(Collider2D other) 
        { 
            if(other.gameObject.TryGetComponent<PlayerView>(out PlayerView _))
                    Controller?.InflictDamage(other);
        } 

        public void Destroy() => Destroy(gameObject);
    }
}