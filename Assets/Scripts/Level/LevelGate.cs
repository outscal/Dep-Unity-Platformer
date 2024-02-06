using Platformer.Main;
using Platformer.Player;
using UnityEngine;

namespace Platformer.Level{
    public class LevelGate : MonoBehaviour
    {
        private LevelService LevelService => GameService.Instance.LevelService;
        private void OnTriggerEnter2D(Collider2D other){
            if(HasHitPlayer(other))
            {
                if (other.isTrigger)
                    return;
                else{
                    LevelService.PlayerReachedGate();
                }
            }
        }
        private bool HasHitPlayer(Collider2D other) => other.GetComponent<PlayerView>() != null;   
    }
}