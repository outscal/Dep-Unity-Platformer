using Platformer.Main;
using UnityEngine;

namespace Platformer.Level{
    public class LevelView : MonoBehaviour{

        private LevelService LevelService => GameService.Instance.LevelService;
        [SerializeField] private Transform[] BackGroundSprites;

        private void LateUpdate() => LevelService.BackgroundParallaxEffect(BackGroundSprites);
    }
}
