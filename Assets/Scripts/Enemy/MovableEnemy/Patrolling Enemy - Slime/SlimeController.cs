using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    public class SlimeController : MovableEnemyController
    {
        private SlimeView slimeView;
        
        public bool IsMovingRight => nextPosition.x > enemyView.transform.position.x;

        #region Patrolling variables
        private Vector3 nextPosition;
        private List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        #endregion
        public SlimeController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            InitializeVariables(enemyScriptableObject);
            InitializeView();
        }

        protected override void InitializeView()
        {
            base.InitializeView();
            slimeView = enemyView as SlimeView;
            enemyView.SetController(this);
        }

        protected override void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            base.InitializeVariables(enemyScriptableObject);
            patrollingPoints = enemyScriptableObject.PatrollingPoints;
            currentPatrolIndex = 0;
            nextPosition = patrollingPoints[currentPatrolIndex];
        }

        public override void Update() => PatrolBehavior();

        #region Patrol Behaviour
        // protected override void PatrolBehavior()
        // {
        //     base.PatrolBehavior();
        //     // if (CanToggleMovementDirection())
        //     // {
        //     //     ToggleNextPatrolPoint();
        //     //     UpdateMovementTowardsNextPosition();
        //     //
        //     // }
        //     // else
        //     // {
        //     //     UpdateMovementTowardsNextPosition();
        //     // }
        // }
        // private void ToggleNextPatrolPoint()
        // {
        //     currentPatrolIndex = (currentPatrolIndex + 1) % patrollingPoints.Count;
        //     nextPosition = patrollingPoints[currentPatrolIndex];
        // }
        //
        // private bool CanToggleMovementDirection()
        // {
        //     var reachedNextPatrolPoint = Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f;
        //     var wallCheck = slimeView.WallCheck;
        //     return reachedNextPatrolPoint || wallCheck;
        // }
        //
        // private void UpdateMovementTowardsNextPosition()
        // {
        //     slimeView.Move(nextPosition, Data.PatrollingSpeed, IsMovingRight);
        //     EnemyMoved();
        // }
        #endregion
    }
}