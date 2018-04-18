using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public abstract class BaseStateMachineState : IStateMachineStateContext
    {
        protected IList<StateMachineTransition> InternalTransitions;
        protected IDictionary<Type, StateMachineTransition> ExternalTransitions;
        protected BaseStateMachineState()
        {
            InternalTransitions = new List<StateMachineTransition>();
            ExternalTransitions = new Dictionary<Type, StateMachineTransition>();
        }
        public virtual void OnEnter(IStateMachineContext context) { }
        public void Run(IStateMachineContext context)
        {
            foreach (var transition in InternalTransitions)
            {
                if (transition.IsExternalTransition)
                    continue;

                transition.Run(context);
            }
        }
        public async Task RunAsync(IStateMachineContext context)
        {
            foreach (var transition in InternalTransitions)
            {
                if (transition.IsExternalTransition)
                    continue;

                await transition.RunAsync(context);
            }
        }
        protected void AfterExecute(BaseStateMachineApplication application) { }
        internal virtual void AddTransition(StateMachineTransition transition)
        {
            if (transition.IsExternalTransition)
            {
                ExternalTransitions[transition.Trigger.GetType()] = transition;
            }
            else InternalTransitions.Add(transition);
        }
        void IStateMachineState.Enter(IStateMachineContext context)
        {
            ((IStateMachineExtendedContext)context).SetCurrentState(this);

            OnEnter(context);
        }

        void IStateMachineState.ResumeExternalActivity<TActivity>(IStateMachineContext context, object arg)
        {
            if (ExternalTransitions.TryGetValue(typeof(TActivity),
                out StateMachineTransition transition))
            {
                transition.Trigger.Execute(context, arg);
                transition.Run(context);
            }
            else
            {
                throw new InvalidTransitionException<TActivity>();
            }
        }
        void IStateMachineState.ResumeExternalActivity<TActivity, TArg>(IStateMachineContext context, TArg arg)
        {
            if (ExternalTransitions.TryGetValue(typeof(TActivity),
                out StateMachineTransition transition))
            {
                transition.Trigger.Execute(context, arg);
                transition.Run(context);
            }
            else
            {
                throw new InvalidTransitionException<TActivity>();
            }
        }
        async Task IStateMachineState.ResumeExternalActivityAsync<TActivity, TArg>(IStateMachineContext context, TArg arg)
        {
            if (ExternalTransitions.TryGetValue(typeof(TActivity),
                out StateMachineTransition transition))
            {
                await transition.Trigger.ExecuteAsync(context, arg);
                await transition.RunAsync(context);
            }
            else
            {
                throw new InvalidTransitionException<TActivity>();
            }
        }
        async Task IStateMachineState.ResumeExternalActivityAsync<TActivity>(IStateMachineContext context, object arg)
        {
            if (ExternalTransitions.TryGetValue(typeof(TActivity),
                out StateMachineTransition transition))
            {
                await transition.Trigger.ExecuteAsync(context, arg);
                await transition.RunAsync(context);
            }
            else
            {
                throw new InvalidTransitionException<TActivity>();
            }
        }
    }
}
