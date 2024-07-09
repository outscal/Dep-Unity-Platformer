using UnityEngine;
using Platformer.Player.Enumerations;

namespace Platformer.AnimationSystem
{
    public class AnimationService
    {
        public void ToggleBoolAnimation(Animator animator, string animationToSwitch , bool isActive)
        {
            animator.SetBool(animationToSwitch, isActive);
        }

        public void PlayTriggerAnimation(Animator animator, string animationToPlay)
        {
            animator.SetTrigger(animationToPlay);
        }
    }
}