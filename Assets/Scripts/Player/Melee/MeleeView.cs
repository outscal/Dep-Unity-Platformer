using System.Threading.Tasks;
using Platformer.Enemy;
using UnityEngine;

namespace Platformer.Player{
    public class MeleeView : MonoBehaviour
    {
        private MeleeController meleeController;
        public async void SetController(MeleeController meleeController){
            this.meleeController = meleeController;
            await Task.Delay((int)(meleeController.meleeScriptableObject.MeleeDuration * 1000));
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.TryGetComponent<EnemyView>(out var enemy))
            {
                meleeController.EnemyHit(enemy);
            }
        }
    }
}
