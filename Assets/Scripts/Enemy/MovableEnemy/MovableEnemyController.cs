using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Platformer.Enemy
{
    public class MovableEnemyController:EnemyController
    {
        private Vector3 nextPosition;
        public List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        
        protected MovableEnemyController(EnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            patrollingPoints = spawnData.PatrolPoints;
            currentPatrolIndex = 0;
            nextPosition = patrollingPoints[currentPatrolIndex];
        }
    
        public bool IsMovingRight => nextPosition.x > enemyView.transform.position.x;
        
        public override void Update()
        {
            
        }
        
        protected virtual void PatrolBehavior()
        {
            if (Vector3.Distance(enemyView.transform.position, nextPosition) < 0.1f)
            {
                ToggleNextPatrolPoint();
            }
            UpdateMovementTowardsNextPosition();
        }

        private void ToggleNextPatrolPoint()
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrollingPoints.Count;
            nextPosition = patrollingPoints[currentPatrolIndex];
        }

        private void UpdateMovementTowardsNextPosition()
        {
            var isMovingRight = nextPosition.x > enemyView.transform.position.x;
            Move(nextPosition, Data.PatrollingSpeed, isMovingRight);
            EnemyMoved();
        }
        
        private void Move(Vector3 position, float speed, bool isMovingRight){
            SetCharacterSpriteDirection(isMovingRight);
            enemyView.SetPosition(Vector3.MoveTowards(enemyView.GetPosition(), position, speed * Time.deltaTime));
        }

        private void SetCharacterSpriteDirection(bool isMovingRight) {
            if (isMovingRight != (enemyView.GetLocalScale().x > 0))
                enemyView.SetLocalScale(new Vector3(-enemyView.GetLocalScale().x, enemyView.GetLocalScale().y, enemyView.GetLocalScale().z));
        }
    }
}