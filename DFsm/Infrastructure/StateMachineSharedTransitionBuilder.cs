using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public class StateMachineSharedTransitionBuilder
    {
        private readonly IStateMachineApplicationContext _application;
        private readonly IStateMachineSharedTransitionContext _sharedTransition;
        public StateMachineSharedTransitionBuilder(IStateMachineApplicationContext application,
            IStateMachineSharedTransitionContext sharedTransition)
        {
            _application = application;
            _sharedTransition = sharedTransition;
        }
        public StateMachineSharedTransitionBuilder Condition(Func<IStateMachineContext, bool> condition)
        {
            _sharedTransition.Condition = condition;
            return this;
        }
        public StateMachineSharedTransitionBuilder Action<TCodeActivity>()
            where TCodeActivity : ICodeActivity, new()
        {
            _sharedTransition.SetAction(new TCodeActivity());
            return this;
        }
        public StateMachineSharedTransitionBuilder Action(Expression<Action<IStateMachineContext>> action)
        {
            _sharedTransition.SetAction(action.Compile());
            return this;
        }
        public StateMachineSharedTransitionBuilder Action(Expression<Func<IStateMachineContext,Task>> action)
        {
            _sharedTransition.SetAction(action.Compile());
            return this;
        }
        public void Target<TState>()
            where TState : BaseStateMachineState, new()
        {
            _sharedTransition.Target = _application.TryAddAndGetState<TState>();
        }
    }
}
