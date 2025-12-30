using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Models.Character
{
    public enum PlayerAnimationState
    {
        // Ground
        GroundIdle = 1,
        GroundWalk,
        GroundRun,
        GroundJump,

        // Swimming
        SwimmingIdle,
        SwimmingMove,

        // Flying
        FlyingIdle,
        FlyingMove
    }

}
