using System.Collections.Generic;
using UnityEngine;

namespace Platformer.AnimationSystem
{
    public class AnimationService
    {
        #region Player Animations
        private Animator player_animator;

        public AnimationService(Animator player_animator) 
        { 
            this.player_animator = player_animator;
        }

        private static readonly Dictionary<PlayerTriggerAnimation, string> AnimationParameters = new Dictionary<PlayerTriggerAnimation, string>
        {
            { PlayerTriggerAnimation.JUMP, "Jump" },
            { PlayerTriggerAnimation.SLIDE, "Slide" },
            { PlayerTriggerAnimation.ATTACK, "Attack" },
            { PlayerTriggerAnimation.TAKE_DAMAGE, "TakeDamage" },
            { PlayerTriggerAnimation.DEATH, "Death" }
        };

        public void PlayPlayerMovementAnimation(bool isRunning) => player_animator.SetBool("Running", isRunning);

        public void PlayPlayerTriggerAnimation(PlayerTriggerAnimation animationToPlay)
        {
            if (AnimationParameters.TryGetValue(animationToPlay, out string animationParameter))
            {
                player_animator.SetTrigger(animationParameter);
            }
            else
            {
                Debug.LogWarning($"No animation parameter found for {animationToPlay}");
            }
        }
        #endregion
    }

    public enum PlayerTriggerAnimation
    {
        JUMP,
        SLIDE,
        ATTACK,
        TAKE_DAMAGE,
        DEATH,
    }
}