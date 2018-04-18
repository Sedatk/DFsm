using System;

namespace DFsm.Infrastructure
{
    public class InvalidTransitionException<TActivity>
        :Exception
        where TActivity : ExternalCodeActivity
    {
        public InvalidTransitionException()
            :base($"Invalid transition {typeof(TActivity).Name} for the current state") {}
    }
}
