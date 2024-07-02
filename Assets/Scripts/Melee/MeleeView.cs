using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Platformer.Melee{
    public class MeleeView : MonoBehaviour
    {
        private MeleeController meleeController;
        private CancellationTokenSource cancellationTokenSource;
        public async void SetController(MeleeController meleeController){
            cancellationTokenSource = new CancellationTokenSource();
            this.meleeController = meleeController;
            await Task.Delay((int)(meleeController.MeleeScriptableObject.MeleeDuration * 1000));
            try {
                await Task.Delay((int)(meleeController.MeleeScriptableObject.MeleeDuration * 1000), cancellationTokenSource.Token);
            } catch (TaskCanceledException) {
                return;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) => meleeController?.InflictDamage(other); 
        

        private void OnDestroy() => cancellationTokenSource?.Cancel();
    }
}
