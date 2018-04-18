using System;

namespace DFsm.Infrastructure
{
    public class StateMachineSharedTransitionsBuilder
    {
        private readonly IStateMachineApplicationContext _application;
        private readonly IStateMachineTransitionContext _transition;

        public StateMachineSharedTransitionsBuilder(IStateMachineApplicationContext application,
            IStateMachineTransitionContext transitionContext)
        {
            _application = application;
            _transition = transitionContext;
        }
        public StateMachineSharedTransitionsBuilder HasSharedTransition(Action<StateMachineSharedTransitionBuilder> builder)
        {
            var sharedTransition = new StateMachineSharedTransition();
            var sharedTransitionBuilder = new StateMachineSharedTransitionBuilder(_application,sharedTransition);

            builder(sharedTransitionBuilder);

            _transition.AddSharedTransition(sharedTransition);

            return this;
        }
    }
}
