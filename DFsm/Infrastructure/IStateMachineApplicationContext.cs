using System.Collections.Generic;

namespace DFsm.Infrastructure
{
    public interface IStateMachineApplicationContext
    {
        IStateMachineContext Context { get; }
        void SetStartupState(IStateMachineState state);
        TState TryAddAndGetState<TState>()
            where TState : IStateMachineState, new();
        IExtensionManager Extensions { get; }
        IDictionary<string, object> GlobalVariables { get; }
        IDictionary<string, object> LocalVariables { get; }
        IStateMachineState CurrentState { get; set; }
    }
}
