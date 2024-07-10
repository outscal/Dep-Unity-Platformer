using Platformer.Level;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Enemy{
    public class SpikeController : ImmovableEnemyController
    {
        public SpikeController(EnemyScriptableObject enemyScriptableObject,EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            enemyView.SetController(this);
        }

    }
}