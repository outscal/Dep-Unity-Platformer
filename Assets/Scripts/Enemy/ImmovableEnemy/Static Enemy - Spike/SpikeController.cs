namespace Platformer.Enemy{
    public class SpikeController : ImmovableEnemyController
    {
        public SpikeController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
        }

    }
}