using System;

namespace DFsm.Infrastructure
{
    public interface IStateMachineTransitionContext : IStateMachineTransition
    {
        ICodeActivity Trigger { set; }
        IStateMachineState Destination { set; }
        Func<IStateMachineContext, bool> Condition { set; }
        bool IsExternalTransition { set; }
        void AddSharedTransition(IStateMachineSharedTransition sharedTransition);
    }
}
