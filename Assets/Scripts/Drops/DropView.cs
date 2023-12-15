using Platformer.Player;
using UnityEngine;

namespace Platformer.Drop{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DropView : MonoBehaviour
    {
        private DropController dropController;

        private SpriteRenderer displayDropSprite;
        [SerializeField] private Sprite[] dropSprites;

        private void Awake() => displayDropSprite = GetComponent<SpriteRenderer>();

        public void SetController(DropController dropController) => this.dropController = dropController;

        public void SetDropSprite(DropType dropType) => displayDropSprite.sprite = dropSprites[(int)dropType];

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(HasHitPlayer(other))
            {
                if (other.isTrigger)
                    return;
                else 
                    dropController.PlayerHit(other.GetComponent<PlayerView>());
            }
            Destroy(gameObject);
        }

        private bool HasHitPlayer(Collider2D other) => other.GetComponent<PlayerView>() != null;
    }
}