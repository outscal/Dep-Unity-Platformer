using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy{
    public class SlimeController : EnemyController
    {
        private readonly SlimeView slimeView;

        #region Patrolling variables
        private Vector3 nextPosition;
        #endregion
        public SlimeController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            slimeView = enemyView as SlimeView;
            enemyView.SetController(this);
            nextPosition = Data.PatrollingPoints[0];
        }

        public void Update() {
            PatrolBehavior();
        }

        #region Patrol Behaviour
        private void PatrolBehavior() {
            var patrollingPoints = enemyScriptableObject.PatrollingPoints;
            if (Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f) {
                ToggleNextPatrolPoint(patrollingPoints);
            }
            UpdateMovementTowardsNextPosition();
        }

        private void ToggleNextPatrolPoint(List<Vector3> patrollingPoints) {
            nextPosition = nextPosition == patrollingPoints[0] ? patrollingPoints[1] : patrollingPoints[0];
        }

        private void UpdateMovementTowardsNextPosition() {
            var isMovingRight = nextPosition.x > enemyView.transform.position.x;
            slimeView.Move(nextPosition, Data.PatrollingSpeed, isMovingRight);
            EnemyMoved();
        }
        #endregion
    }
}