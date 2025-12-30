using Assets.Scripts.Models.Character;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Scripts.Player
{
    public static class PlayerAnimator
    {
        public static void ApplyAnimatorState(PlayerAnimationState state, Animator animator)
        {
            animator.SetBool("IsJumping", state == PlayerAnimationState.GroundJump);

            animator.SetBool("IsSwimming",
                state == PlayerAnimationState.SwimmingIdle ||
                state == PlayerAnimationState.SwimmingMove);

            animator.SetBool("IsFlying",
                state == PlayerAnimationState.FlyingIdle ||
                state == PlayerAnimationState.FlyingMove);

            float speed = state switch
            {
                PlayerAnimationState.GroundIdle => 0,
                PlayerAnimationState.GroundWalk => 0.5f,
                PlayerAnimationState.GroundRun => 1f,
                PlayerAnimationState.SwimmingIdle => 0,
                PlayerAnimationState.SwimmingMove => 0.6f,
                PlayerAnimationState.FlyingIdle => 0,
                PlayerAnimationState.FlyingMove => 0.8f,
                _ => 0
            };

            animator.SetFloat("Speed", speed);
        }
    }
}
