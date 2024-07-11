using System.Collections;
using Platformer.Services;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class EnemyCombatController
    {
        private EnemyController owner;
        private int attackCoroutineId;
        
        public EnemyCombatController(EnemyController owner)
        {
            this.owner = owner;
        }

        public IEnumerator RepeatedlyAttackPlayer(Collider2D playerCollider, float delay)
        {
            while (true)
            {
                OnEnemyAttack(playerCollider);
                yield return new WaitForSeconds(delay);
            }
        }
        public void OnEnemyAttack(Collider2D other)
        {
            owner.OnEnemyAttack(other);
        }
        
        public void StartAttackCoroutine(Collider2D playerCollider)
        {
            if(owner.enemyScriptableObject is MovableEnemyScriptableObject movableScriptableObject)
                attackCoroutineId = CoroutineService.StartCoroutine(RepeatedlyAttackPlayer(playerCollider, movableScriptableObject.DelayAfterAttack));
        }

        public void StopAttackCoroutine()
        {
            CoroutineService.StopCoroutine(attackCoroutineId);
        }
    }
}