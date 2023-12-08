using System.Collections.Generic;
using Platformer.Main;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy{
    public class EnemyView : MonoBehaviour
    {
        public EnemyController Controller { get; private set; }
        private CircleCollider2D rangeTriggerCollider;
        [SerializeField] private SpriteRenderer detectableRange;

        private bool isPatrolling = false;
        private List<Vector3> patrollingPoints; 
        private Vector3 nextPosition;

        public void SetController(EnemyController controllerToSet) => Controller = controllerToSet;

        public void SetDetectableZone(float radiusToSet)
        {
            SetRangeColliderRadius(radiusToSet);
            SetRangeImageRadius(radiusToSet);
        }

        private void SetRangeColliderRadius(float radiusToSet)
        {
            if (rangeTriggerCollider != null)
                rangeTriggerCollider.radius = radiusToSet;
        }

        private void SetRangeImageRadius(float radiusToSet){
            if(detectableRange != null)
                detectableRange.transform.localScale = new Vector3(radiusToSet, radiusToSet, 1);
        }

        public void TakeDamage(int damageToInflict) => Controller.TakeDamage(damageToInflict);

        public void Destroy(){
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other){
            if(other.GetComponent<PlayerView>() != null){
                GameService.Instance.PlayerService.TakeDamage(10);
            }
        }

        public void Patrol(List<Vector3> patrollingPoints){
            this.patrollingPoints = patrollingPoints;
            nextPosition = patrollingPoints[0];
            isPatrolling = true;
        }

        private void Update(){
            if(isPatrolling){
                if (Vector3.Distance(transform.position, nextPosition) < 0.1f)
                    nextPosition = nextPosition == patrollingPoints[0] ? patrollingPoints[1] : patrollingPoints[0];
                transform.position = Vector3.MoveTowards(transform.position, nextPosition, Controller.Data.PatrollingSpeed * Time.deltaTime);
            }
        }
    }
}