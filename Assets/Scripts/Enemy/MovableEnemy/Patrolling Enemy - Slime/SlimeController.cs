using System.Collections.Generic;
using UnityEngine;

namespace Platformer.Enemy
{
    public class SlimeController : MovableEnemyController
    {
        
        public bool IsMovingRight => nextPosition.x > enemyView.transform.position.x;

        #region Patrolling variables
        private Vector3 nextPosition;
        private List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        #endregion
        public SlimeController(MovableEnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            InitializeVariables(enemyScriptableObject);
            InitializeView(spawnData);
        }

        protected override void InitializeView(EnemySpawnData spawnData)
        {
            base.InitializeView(spawnData);
            enemyView.SetController(this);
            patrollingPoints = spawnData.PatrolPoints;
        }

        protected override void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            base.InitializeVariables(enemyScriptableObject);
           
            currentPatrolIndex = 0;
            nextPosition = patrollingPoints[currentPatrolIndex];
        }

        public override void Update() => PatrolBehavior();
    }
}