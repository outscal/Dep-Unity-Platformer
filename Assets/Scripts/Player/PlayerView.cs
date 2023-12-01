using UnityEngine;

namespace Platformer.Player{
    public class PlayerView : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer characterSprite;
        public Animator playerAnimator => animator;
        public PlayerController Controller { get; private set; }

        public void SetController(PlayerController controllerToSet) => Controller = controllerToSet;

        public void Move(float horizontalInput) => characterSprite.flipX = horizontalInput < 0;
    }
}