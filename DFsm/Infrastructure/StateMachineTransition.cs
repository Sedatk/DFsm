using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public class StateMachineTransition : IStateMachineTransitionContext
    {
        public StateMachineTransition()
        {
            _sharedTransitions = new List<IStateMachineSharedTransition>();
        }
        public Func<IStateMachineContext, bool> Condition { get; private set; }
        public bool IsExternalTransition { get; private set; }
        public ICodeActivity Trigger { get; private set; }
        bool IStateMachineTransitionContext.IsExternalTransition { set { IsExternalTransition = value; } }
        readonly IList<IStateMachineSharedTransition> _sharedTransitions;
        public void Run(IStateMachineContext context)
        {
            foreach (var sharedTransition in _sharedTransitions)
            {
                sharedTransition.Run(context);
            }
        }
        public async Task RunAsync(IStateMachineContext context)
        {
            foreach (var sharedTransition in _sharedTransitions)
            {
                if (sharedTransition.Condition != null)
                {
                    if (!sharedTransition.Condition(context))
                        continue;

                    await sharedTransition.RunAsync(context);
                    break;
                }

                await sharedTransition.RunAsync(context);
                break;
            }
        }
        IStateMachineState IStateMachineTransitionContext.Destination
        {
            set
            {
                if (_sharedTransitions.Any())
                    _sharedTransitions.Clear();

                var destinationTransition = new StateMachineSharedTransition();
                ((IStateMachineSharedTransitionContext)destinationTransition).Target = value;
                _sharedTransitions.Add(destinationTransition);
            }
        }
        void IStateMachineTransitionContext.AddSharedTransition(IStateMachineSharedTransition sharedTransition)
        {
            _sharedTransitions.Add(sharedTransition);
        }
        ICodeActivity IStateMachineTransitionContext.Trigger
        {
            set
            {
                Trigger = value;

                if (Trigger.GetType().IsSubclassOf(typeof(ExternalCodeActivity)))
                    IsExternalTransition = true;
            }
        }
        Func<IStateMachineContext, bool> IStateMachineTransitionContext.Condition
        {
            set
            {
                if (_sharedTransitions.Any())
                    _sharedTransitions.Clear();

                var destinationTransition = new StateMachineSharedTransition();
                ((IStateMachineSharedTransitionContext)destinationTransition).Condition = value;

                _sharedTransitions.Add(destinationTransition);
                Condition = value;
            }
        }
    }
}
