using System;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    class StateMachineSharedTransition:IStateMachineSharedTransitionContext
    {
        private Func<IStateMachineContext, bool> _condition;
        private IStateMachineState _destination;
        private ICodeActivity _codeActivityAction;
        private Action<IStateMachineContext> _action;
        private Func<IStateMachineContext, Task> _asyncAction; 
        public StateMachineSharedTransition()
        {
            
        }

        IStateMachineState IStateMachineSharedTransitionContext.Target
        {
            set { _destination = value; }
        }

        Func<IStateMachineContext, bool> IStateMachineSharedTransitionContext.Condition
        {
            set { _condition = value; }
        }
        Func<IStateMachineContext, bool> IStateMachineSharedTransition.Condition => _condition;

        void IStateMachineSharedTransitionContext.SetAction(ICodeActivity action)
        {
            _codeActivityAction = action;
        }

        void IStateMachineSharedTransitionContext.SetAction(Action<IStateMachineContext> action)
        {
            _action = action;
        }
        void IStateMachineSharedTransitionContext.SetAction(Func<IStateMachineContext,Task> action)
        {
            _asyncAction = action;
        }
        public void Run(IStateMachineContext context)
        {
            if (_codeActivityAction != null)
            {
                _codeActivityAction.Execute(context, null);
            }
            else { 
                _action?.Invoke(context);
            }
            _destination.Enter(context);
        }
        public async Task RunAsync(IStateMachineContext context)
        {
            if (_codeActivityAction != null)
            {
                await _codeActivityAction.ExecuteAsync(context, null);
            }
            else if (_asyncAction != null)
            {
                await _asyncAction(context);
            }
            else
            {
                _action?.Invoke(context);
            }

            _destination.Enter(context);
            await Task.Yield();
        }
    }
}
