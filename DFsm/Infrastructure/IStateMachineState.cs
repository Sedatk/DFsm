using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public interface IStateMachineState
    {
        void ResumeExternalActivity<TActivity>(IStateMachineContext context, object arg)
            where TActivity : ExternalCodeActivity;
        void ResumeExternalActivity<TActivity, TArg>(IStateMachineContext context, TArg arg)
            where TActivity : ExternalCodeActivity;
        Task ResumeExternalActivityAsync<TActivity, TArg>(IStateMachineContext context, TArg arg)
            where TActivity : ExternalCodeActivity;
        Task ResumeExternalActivityAsync<TActivity>(IStateMachineContext context, object arg)
            where TActivity : ExternalCodeActivity;
        void Run(IStateMachineContext context);
        Task RunAsync(IStateMachineContext context);
        void Enter(IStateMachineContext context);
    }
}
