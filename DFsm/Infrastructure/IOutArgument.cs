namespace DFsm.Infrastructure
{
    public interface IOutArgument<T>
    {
        void Set(IStateMachineContext context, T value);
        string Name { get; }
    }
}
