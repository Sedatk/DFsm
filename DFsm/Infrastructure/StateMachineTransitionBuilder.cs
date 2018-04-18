using System;

namespace DFsm.Infrastructure
{
    public class StateMachineTransitionBuilder
    {
        private readonly IStateMachineApplicationContext _applicationContext;
        private readonly IStateMachineTransitionContext _transition;
        public StateMachineTransitionBuilder(IStateMachineApplicationContext application
            , IStateMachineTransitionContext transition)
        {
            _applicationContext = application;
            _transition = transition;
        }
        public void Target<TState>()
            where TState : BaseStateMachineState, new()
        {
            _transition.Destination = _applicationContext.TryAddAndGetState<TState>();
        }
        public StateMachineSharedTransitionsBuilder HasSharedTransition(Action<StateMachineSharedTransitionBuilder> builder)
        {
            var sharedTransition = new StateMachineSharedTransition();
            var sharedTransitionBuilder = new StateMachineSharedTransitionBuilder(_applicationContext, sharedTransition);
            builder(sharedTransitionBuilder);

            _transition.AddSharedTransition(sharedTransition);

            return new StateMachineSharedTransitionsBuilder(_applicationContext, _transition);
        }
        public StateMachineTransitionBuilder Trigger<TCodeActivity>()
            where TCodeActivity : ICodeActivity, new()
        {
            _transition.Trigger = new TCodeActivity();
            return this;
        }
    }
}
