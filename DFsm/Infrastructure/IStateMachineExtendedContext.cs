namespace DFsm.Infrastructure
{
    interface IStateMachineExtendedContext:IStateMachineContext
    {
        void SetCurrentState(IStateMachineState state);
    }
}
