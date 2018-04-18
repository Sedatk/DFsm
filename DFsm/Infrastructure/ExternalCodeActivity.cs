using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public abstract class ExternalCodeActivity : CodeActivity
    {

    }
    public abstract class ExternalCodeActivity<TArg> : ExternalCodeActivity
    {
        protected abstract void Execute(IStateMachineContext context, TArg argument);
        protected override sealed void Execute(IStateMachineContext context, object argument)
        {
            Execute(context, (TArg)argument);
        }

        protected abstract Task ExecuteAsync(IStateMachineContext context, TArg argument);
        protected override sealed async Task ExecuteAsync(IStateMachineContext context, object argument)
        {
            await ExecuteAsync(context, (TArg)argument);
        }
    }
}
