using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; private set; }

        public virtual void SetController(EnemyController controllerToSet) => Controller = controllerToSet;

        private void OnTriggerEnter2D(Collider2D other) 
        { 
            if(other.gameObject.TryGetComponent<PlayerView>(out PlayerView _))
                    Controller?.InflictDamage(other);
        } 

        public void Destroy() => Destroy(gameObject);
    }
}