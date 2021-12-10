using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheLoneHerder
{
    public abstract class FenceBaseState
    {
        public abstract void EnterState(FenceStateManager fence);
        public abstract void ExitState();
    }
}