namespace DFsm.Infrastructure
{
    public class StateMachineOutArgument<T> : IOutArgument<T>
    {
        public string Name { get; }

        public void Set(IStateMachineContext context, T value)
        {
            context.SetArgument(Name, value);
        }

        public StateMachineOutArgument(string argumentName)
        {
            Name = argumentName;
        }
    }
}
