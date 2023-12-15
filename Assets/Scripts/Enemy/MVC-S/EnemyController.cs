using Platformer.Main;
using UnityEngine;

namespace Platformer.Enemy
{
    public class EnemyController
    {
        protected EnemyScriptableObject enemyScriptableObject;
        protected EnemyView enemyView;
        protected int currentHealth;

        #region Getters
        public EnemyScriptableObject Data => enemyScriptableObject;
        public EnemyView EnemyView => enemyView;
        #endregion

        public EnemyController(EnemyScriptableObject enemyScriptableObject)
        {
            this.enemyScriptableObject = enemyScriptableObject;
            InitializeView();
            InitializeVariables();
        }

        private void InitializeView()
        {
            enemyView = Object.Instantiate(enemyScriptableObject.Prefab);
            enemyView.transform.position = enemyScriptableObject.SpawnPosition;
            enemyView.SetDetectableZone(enemyScriptableObject.RangeRadius);
        }

        private void InitializeVariables() => currentHealth = enemyScriptableObject.MaximumHealth;

        public virtual void TakeDamage(int damageToInflict){
            currentHealth -= damageToInflict;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        protected virtual void Die() 
        {
            GameService.Instance.EnemyService.EnemyDied(this);
            enemyView.Destroy();
        }
    }
}