namespace Platformer.Enemy{
    public class PatrolmanController : EnemyController
    {
        public PatrolmanController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
            enemyView.Patrol(enemyScriptableObject.PatrollingPoints);
        }
    }
}