using System.Collections.Generic;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class SlimeController : MovableEnemyController, IDamagable
    {
        
        public bool IsMovingRight => nextPosition.x > enemyView.transform.position.x;

        #region Patrolling variables
        private Vector3 nextPosition;
        private List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        #endregion
        public SlimeController(MovableEnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            InitializeView(spawnData);
        }

        protected override void InitializeView(EnemySpawnData spawnData)
        {
            base.InitializeView(spawnData);
            enemyView.SetController(this);
        }

        public override void Update() => PatrolBehavior();
    }
}