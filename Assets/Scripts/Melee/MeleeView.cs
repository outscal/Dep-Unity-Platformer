using System.Threading.Tasks;
using UnityEngine;

namespace Platformer.Melee{
    public class MeleeView : MonoBehaviour
    {
        private MeleeController meleeController;
        public async void SetController(MeleeController meleeController){
            this.meleeController = meleeController;
            await Task.Delay((int)(meleeController.MeleeScriptableObject.MeleeDuration * 1000));
            if(gameObject != null) Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other) => meleeController?.InflictDamage(other);
    }
}
