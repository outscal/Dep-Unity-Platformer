using Platformer.Level;

namespace Platformer.Enemy{
    public class SpikeController : ImmovableEnemyController
    {
        public SpikeController(EnemyScriptableObject enemyScriptableObject,EnemySpawnData spawnData) : base(enemyScriptableObject, spawnData)
        {
            enemyView.SetController(this);
        }

    }
}