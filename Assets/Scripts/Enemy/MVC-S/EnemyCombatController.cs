using System.Collections;
using Platformer.AnimationSystem;
using Platformer.Main;
using Platformer.Player;
using Platformer.Services;
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
            while (owner.CurrentHealth > 0)
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
            attackCoroutineId = CoroutineService.StartCoroutine(RepeatedlyAttackPlayer(playerCollider, owner.enemyScriptableObject.DelayAfterAttack));
        }

        public void StopAttackCoroutine()
        {
            CoroutineService.StopCoroutine(attackCoroutineId);
        }
    }
}