using System.Collections;
using System.Threading.Tasks;
using Platformer.AnimationSystem;
using Platformer.InputSystem;
using Platformer.Main;
using Platformer.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Platformer.Player
{
    public class PlayerAnimationController
    {
        private AnimationService AnimationService => GameService.Instance.AnimationService;
        
        public void PlayMovementAnimation(PlayerState currentPlayerState) => AnimationService.PlayPlayerMovementAnimation(currentPlayerState);
        public void PlayJumpAnimation() => AnimationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.JUMP);
        public void PlayDamageAnimation() => AnimationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.TAKE_DAMAGE);
        public void PlayDeathAnimation() => AnimationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.DEATH);
        public void PlaySlideAnimation() => AnimationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.SLIDE);
        public void PlayAttackAnimation() => AnimationService.PlayPlayerTriggerAnimation(PlayerTriggerAnimation.ATTACK);
    }
}