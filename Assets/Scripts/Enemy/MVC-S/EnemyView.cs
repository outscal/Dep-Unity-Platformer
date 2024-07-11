using Platformer.Player;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; private set; }
        
        [HideInInspector]public Animator Animator;

        public virtual void SetController(EnemyController controllerToSet)
        {
            Controller = controllerToSet;
            Animator = GetComponent<Animator>();
            
        }

        public Vector3 GetPosition() => transform.position;
        public void SetPosition(Vector3 positionToSet) => transform.position = positionToSet; 
        public Vector3 GetLocalScale() => transform.localScale;

        public void SetLocalScale(Vector3 localScale) => transform.localScale = localScale;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent<PlayerView>(out var _))
            {
                Controller.OnCollisionWithPlayer(other);
            }
        }
        
        public void TakeDamage(int damageToTake)
        {
            if(Controller is IDamagable controller)
                controller.TakeDamage(damageToTake);
        }

        public void Destroy() => Destroy(gameObject);
    }
}