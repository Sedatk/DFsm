using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public abstract class CodeActivity : ICodeActivity
    {
        protected abstract void Execute(IStateMachineContext context, object args);
        protected abstract Task ExecuteAsync(IStateMachineContext context, object args);
        void ICodeActivity.Execute(IStateMachineContext context, object args)
        {
            Execute(context, args);
        }

        async Task ICodeActivity.ExecuteAsync(IStateMachineContext context, object args)
        {
            await ExecuteAsync(context, args);
        }
    }
}
