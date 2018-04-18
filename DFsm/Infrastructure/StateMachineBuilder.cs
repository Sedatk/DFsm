using System;

namespace DFsm.Infrastructure
{
    public class StateMachineBuilder
    {
        public StateMachineBuilder(IStateMachineApplicationContext application)
        {
            _application = application;
        }
        private readonly IStateMachineApplicationContext _application;
        public StateMachineBuilder HasState<TState>(Action<StateMachineStateBuilder> stateBuilder)
            where TState : BaseStateMachineState, new()
        {
            var state = _application.TryAddAndGetState<TState>();
            stateBuilder(new StateMachineStateBuilder(_application, state));
            return this;
        }
        public StateMachineBuilder HasState<TState>()
            where TState : BaseStateMachineState, new()
        {
            _application.TryAddAndGetState<TState>();
            return this;
        }
        public StateMachineBuilder Initalize(Action<IStateMachineContext> context)
        {
            context(_application.Context);
            return this;
        }
    }
}
