using System.Collections.Generic;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class SlimeController : MovableEnemyController, IDamagable
    {
        
        #region Patrolling variables
        private Vector3 nextPosition;
        private List<Vector3> patrollingPoints;
        private int currentPatrolIndex;
        #endregion
        public SlimeController(SlimeScriptableObject enemyScriptableObject, EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            InitializeView(spawnData);
            InitializeVariables(enemyScriptableObject);
            currentHealth = enemyScriptableObject.MaximumHealth;

        }

        protected override void InitializeView(EnemySpawnData spawnData)
        {
            base.InitializeView(spawnData);
            enemyView.SetController(this);
        }

        
        public int CurrentHealth()
        { 
            currentHealth = ClampHealth(currentHealth); 
            UpdateHealthService();
            return currentHealth;
        }

        public int ClampHealth(int value)
        {
            if(Data is SlimeScriptableObject data)
                return Mathf.Clamp(value, 0, data.MaximumHealth);
            return 0;
        }

        public void UpdateHealthService()
        {
            float healthRatio = CalculateHealthRatio();
            EnemyService.UpdateEnemyHealth(this, healthRatio);
        }
        
        public float CalculateHealthRatio()
        {
            if(Data is SlimeScriptableObject data)
                return (float)currentHealth / data.MaximumHealth;
            return 0;
        }
        
        
        public virtual void TakeDamage(int damageToInflict){
            currentHealth -= damageToInflict;
            if(currentHealth <= 0)
            {
                currentHealth = 0;
                Die();
            }
        }

        public override void Update()
        {
            PatrolBehavior();
            CurrentHealth();
        }
    }
}