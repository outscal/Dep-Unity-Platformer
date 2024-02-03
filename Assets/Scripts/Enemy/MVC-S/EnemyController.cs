using Platformer.Main;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class EnemyController
    {
        #region Enemy Service
        protected EnemyService EnemyService => GameService.Instance.EnemyService;
        #endregion

        protected EnemyScriptableObject enemyScriptableObject;
        protected EnemyView enemyView;

        #region Health 
        protected int currentHealth;
        public int CurrentHealth {
            get => currentHealth;
            private set {
                currentHealth = Mathf.Clamp(value, 0, enemyScriptableObject.MaximumHealth);
                if(this is not SpikeController)
                    EnemyService.UpdateEnemyHealth(this, (float)currentHealth / enemyScriptableObject.MaximumHealth);
            }
        }
        #endregion

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

        protected virtual void InitializeView()
        {
            enemyView = Object.Instantiate(enemyScriptableObject.Prefab);
            enemyView.transform.position = enemyScriptableObject.SpawnPosition;
        }

        private void InitializeVariables() => CurrentHealth = enemyScriptableObject.MaximumHealth;


        #region Damage

        //spike does not take damage, what to do here????
        public virtual void TakeDamage(int damageToInflict){
            CurrentHealth -= damageToInflict;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        // all enemies inflict damage, so this function belongs in the base enemy controller class
        public virtual void InflictDamage(Collider2D other) => other.GetComponent<IDamagable>()?.TakeDamage(enemyScriptableObject.DamageToInflict);
        #endregion

        // what to do about this function?????
        public void EnemyMoved() => EnemyService.EnemyMoved(this);


        // belongs here
        protected virtual void Die() 
        {
            GameService.Instance.EnemyService.EnemyDied(this);
            enemyView.Destroy();
        }
    }
}