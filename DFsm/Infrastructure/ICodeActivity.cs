using System.Threading.Tasks;

namespace DFsm.Infrastructure
{
    public interface ICodeActivity
    {
        void Execute(IStateMachineContext context, object args);
        Task ExecuteAsync(IStateMachineContext context, object args);
    }
}
