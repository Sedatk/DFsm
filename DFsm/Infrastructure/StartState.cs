using System.Linq;

namespace DFsm.Infrastructure
{
    class StartState : BaseStateMachineState
    {
        internal override void AddTransition(StateMachineTransition transition)
        {
            if (InternalTransitions.Any())
                InternalTransitions.Clear();

            base.AddTransition(transition);
        }
    }
}
