using UnityEngine;

namespace Platformer.Enemy{
    public class SpikeController : EnemyController
    {
        public SpikeController(EnemyScriptableObject enemyScriptableObject) : base(enemyScriptableObject)
        {
            enemyView.SetController(this);
        }
    }
}