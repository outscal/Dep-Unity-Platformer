using System.Collections.Generic;
using Platformer.Enemy;
using Platformer.Main;
using Platformer.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer.UI{
    public class WorldSpaceCanvasController : MonoBehaviour
    {
        #region UI Service Reference
        private UIService UIService => GameService.Instance.UIService;
        #endregion

        [Header("Gameplay View")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Transform gameplayUIView;

        private Dictionary<EnemyController, Slider> enemyHealthBars;


        public void Initialise() => enemyHealthBars = new Dictionary<EnemyController, Slider>();

        public void UpdateEnemyHealth(EnemyController enemy, float healthRatio){
            if (enemyHealthBars.ContainsKey(enemy)){
                SetEnemyHealth(enemyHealthBars[enemy], healthRatio);
            }
            else {
                var newHealthBar = Instantiate(healthBar, gameplayUIView);
                newHealthBar.transform.localScale = new Vector3(0.007f, 0.005f, 1.0f);
                enemyHealthBars.Add(enemy, newHealthBar);
                SetHealthBarPosition(enemy);
                SetEnemyHealth(newHealthBar, healthRatio);
            }
        }

        private void SetHealthBarPosition(EnemyController enemy){
            if (enemyHealthBars.TryGetValue(enemy, out var healthBar) && healthBar != null) {
                if (enemy is IDamagable && enemy.Data is MovableEnemyScriptableObject movableEnemy)
                {
                    var offset = new Vector3(0, movableEnemy.HealthbarPositionOffset, 0);
                    healthBar.transform.position = enemy.EnemyView.transform.position + offset;
                }
            }
        }

        private void SetEnemyHealth(Slider healthBar, float healthRatio) => healthBar.value = healthRatio;
        
        private void DisableHealthBar(EnemyController enemy){
            var healthBar = enemyHealthBars[enemy];
            enemyHealthBars.Remove(enemy);
            Destroy(healthBar.gameObject);
        }

        public void OnEnemyMoved(EnemyController enemy) => SetHealthBarPosition(enemy);
        public void OnEnemyDied(EnemyController enemy) => DisableHealthBar(enemy);
    }
}
