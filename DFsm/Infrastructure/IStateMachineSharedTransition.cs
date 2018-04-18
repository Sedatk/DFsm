using System;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public interface IStateMachineSharedTransition
    {
        Func<IStateMachineContext, bool> Condition { get; }
        void Run(IStateMachineContext context);
        Task RunAsync(IStateMachineContext context);
    }
}
