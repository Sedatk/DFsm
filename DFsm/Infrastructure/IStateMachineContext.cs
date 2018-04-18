namespace DFsm.Infrastructure
{
    public interface IStateMachineContext
    {
        T GetExtension<T>() where T : class;
        TVariable GetGlobalVariable<TVariable>(string variableName);
        void SetGlobalVariable<TVariable>(string variableName, TVariable value);
        TVariable GetArgument<TVariable>(string variableName);
        void SetArgument<TVariable>(string variableName, TVariable value);
    }
}
