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
        public MovableEnemyController MovableEnemyController;

        #region Health 
        protected int currentHealth;
        public int CurrentHealth
        {
            get => currentHealth;
            private set
            {
                currentHealth = ClampHealth(value);
                if (ShouldUpdateHealth())
                {
                    UpdateHealthService();
                }
            }
        }
        
        

        private int ClampHealth(int value)
        {
            return Mathf.Clamp(value, 0, enemyScriptableObject.MaximumHealth);
        }

        private bool ShouldUpdateHealth()
        {
            return this is not SpikeController;
        }

        private void UpdateHealthService()
        {
            float healthRatio = CalculateHealthRatio();
            EnemyService.UpdateEnemyHealth(this, healthRatio);
        }

        private float CalculateHealthRatio()
        {
            return (float)currentHealth / enemyScriptableObject.MaximumHealth;
        }
        #endregion

        #region Getters
        public EnemyScriptableObject Data => enemyScriptableObject;
        public EnemyView EnemyView => enemyView;
        #endregion

        public EnemyController(EnemyScriptableObject enemyScriptableObject)
        {
            enemyView = Object.Instantiate(enemyScriptableObject.Prefab);
            InitializeVariables(enemyScriptableObject);
            InitializeView();
        }

        protected virtual void InitializeView()
        {
            enemyView.transform.position = enemyScriptableObject.SpawnPosition;
        }

        protected virtual void InitializeVariables(EnemyScriptableObject enemyScriptableObject)
        {
            this.enemyScriptableObject = enemyScriptableObject;
            CurrentHealth = enemyScriptableObject.MaximumHealth;
        } 


        #region Damage
        public virtual void TakeDamage(int damageToInflict){
            CurrentHealth -= damageToInflict;
            if(CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                Die();
            }
        }

        public virtual void InflictDamage(Collider2D other) => other.GetComponent<IDamagable>()?.TakeDamage(enemyScriptableObject.DamageToInflict);
        #endregion

        public void EnemyMoved() => EnemyService.EnemyMoved(this);

        protected virtual void Die() 
        {
            GameService.Instance.EnemyService.EnemyDied(this);
            enemyView.Destroy();
        }

        public virtual void Update()
        {
            
        }
    }
}