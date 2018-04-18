namespace DFsm.Infrastructure
{
    public class StateMachineContext
        : IStateMachineContext
    {
        readonly IStateMachineApplicationContext _applicationContext;
        public StateMachineContext(IStateMachineApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }
        public T GetExtension<T>() where T : class
            => _applicationContext.Extensions.GetExtension<T>();
        public TVariable GetGlobalVariable<TVariable>(string variableName)
        {
            if (_applicationContext.GlobalVariables.TryGetValue(variableName, out object variable))
                return (TVariable)variable;

            return default(TVariable);
        }
        public void SetGlobalVariable<TVariable>(string variableName, TVariable value)
            => _applicationContext.GlobalVariables[variableName] = value;
        public TVariable GetArgument<TVariable>(string variableName)
        {
            if (_applicationContext.LocalVariables.TryGetValue(variableName, out object variable))
                return (TVariable)variable;
            return default(TVariable);
        }
        public void SetArgument<TVariable>(string variableName, TVariable value)
            => _applicationContext.LocalVariables[variableName] = value;
    }
}
