using Yarde.EventDispatcher;

namespace Yarde.Code.Flows
{
    public abstract class RootFlowBase : BaseFlow
    {
        protected RootFlowBase(IDispatcher dispatcher) : base(dispatcher) { }
    }
}
