using System;

namespace DFsm.Infrastructure
{
    public class StateMachineStateBuilder
    {
        private readonly IStateMachineApplicationContext _applicationConfiguration;
        private readonly BaseStateMachineState _state;
        public StateMachineStateBuilder(IStateMachineApplicationContext application, BaseStateMachineState state)
        {
            _applicationConfiguration = application;
            _state = state;
        }
        public StateMachineStateBuilder HasTransition(Action<StateMachineTransitionBuilder> builder)
        {
            var transition = new StateMachineTransition();
            builder(new StateMachineTransitionBuilder(_applicationConfiguration, transition));
            _state.AddTransition(transition);
            return this;
        }
        public StateMachineStateBuilder IsStartupState()
        {
            _applicationConfiguration.SetStartupState(_state);
            return this;
        }
    }
}
