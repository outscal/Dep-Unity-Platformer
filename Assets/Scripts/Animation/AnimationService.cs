using UnityEngine;
using Platformer.Player;

namespace Platformer.AnimationSystem
{
    public class AnimationService
    {
        private Animator player_animator;

        public AnimationService(Animator player_animator) 
        { 
            this.player_animator = player_animator;
        }

        public void PlayPlayerMovementAnimation(PlayerState current_player_state)
        {
            player_animator.SetBool(PlayerAnimation.RUNNING.ToString(), current_player_state == PlayerState.RUNNING);
        }

        public void PlayPlayerTriggerAnimation(PlayerTriggerAnimation animationToPlay)
        {
            player_animator.SetTrigger(animationToPlay.ToString());
        }
    }
}