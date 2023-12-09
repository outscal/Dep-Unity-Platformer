using System.Threading.Tasks;
using Platformer.Enemy;
using Platformer.Main;
using UnityEngine;

namespace Platformer.Player{
    public class MeleeView : MonoBehaviour
    {
        private MeleeController meleeController;
        private bool isEnemyHit = false;
        public async void SetController(MeleeController meleeController){
            this.meleeController = meleeController;
            await Task.Delay((int)(meleeController.meleeScriptableObject.MeleeDuration * 1000));
            if(!isEnemyHit) GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.PLAYER_SLASH);
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<EnemyView>(out var enemy))
            {
                isEnemyHit = true;
                meleeController.EnemyHit(enemy);
            }
        }
    }
}
