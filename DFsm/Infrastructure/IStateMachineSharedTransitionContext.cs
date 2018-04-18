using System;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public interface IStateMachineSharedTransitionContext:IStateMachineSharedTransition
    {
        Func<IStateMachineContext, bool> Condition { set; }
        IStateMachineState Target { set; }
        void SetAction(ICodeActivity action);
        void SetAction(Action<IStateMachineContext> action);
        void SetAction(Func<IStateMachineContext,Task> action);
    }
}
