using System.Collections;
using System.Collections.Generic;
using Platformer.AnimationSystem;
using Platformer.Level;
using Platformer.Main;
using Platformer.Player;
using Platformer.Services;
using Platformer.Utilities;
using UnityEngine;

namespace Platformer.Enemy
{
    public class EnemyController
    {
        #region Enemy Service
        public EnemyService EnemyService => GameService.Instance.EnemyService;
        
        #endregion

        public EnemyScriptableObject enemyScriptableObject;
        protected EnemyView enemyView;
        private int attackCoroutineId;


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
        
        
        public EnemyController(EnemyScriptableObject enemyScriptableObject, EnemySpawnData spawnData)
        {
            enemyView = Object.Instantiate(enemyScriptableObject.Prefab);
            InitializeVariables(enemyScriptableObject);
            InitializeView(spawnData);
        }

        protected virtual void InitializeView(EnemySpawnData spawnData)
        {
            enemyView.transform.position = spawnData.SpawnPosition;
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

        public virtual void OnCollisionWithPlayer(Collider2D other) => OnEnemyAttack(other);

        //public virtual void OnCollisionEndWithPlayer() => StopAttackCoroutine();

        public virtual void OnEnemyAttack(Collider2D other)
        {
            InflictDamage(other);
        }

        public virtual void InflictDamage(Collider2D other)
        {
            other.GetComponent<PlayerView>()?.TakeDamage(enemyScriptableObject.DamageToInflict);
        }

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