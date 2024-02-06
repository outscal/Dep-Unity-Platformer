using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy{
    public class SlimeController : EnemyController
    {
        private readonly SlimeView slimeView;

        public bool IsMovingRight => nextPosition.x > enemyView.transform.position.x;

        #region Patrolling variables
        private Vector3 nextPosition;
        #endregion
        public SlimeController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            slimeView = enemyView as SlimeView;
            enemyView.SetController(this);
            nextPosition = Data.PatrollingPoints[0];
        }

        public void Update() => PatrolBehavior();

        #region Patrol Behaviour
        private void PatrolBehavior() {
            var patrollingPoints = enemyScriptableObject.PatrollingPoints;
            if(CanToggleMovementDirection()) ToggleNextPatrolPoint(patrollingPoints);
            UpdateMovementTowardsNextPosition();
        }

        private bool CanToggleMovementDirection(){
            var reachedNextPatrolPoint = Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f;
            var wallCheck = slimeView.WallCheck;
            return reachedNextPatrolPoint || wallCheck;
        }

        private void ToggleNextPatrolPoint(List<Vector3> patrollingPoints) {
            nextPosition = nextPosition == patrollingPoints[0] ? patrollingPoints[1] : patrollingPoints[0];
        }

        private void UpdateMovementTowardsNextPosition() {
            slimeView.Move(nextPosition, Data.PatrollingSpeed, IsMovingRight);
            EnemyMoved();
        }
        #endregion
    }
}