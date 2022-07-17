using Cysharp.Threading.Tasks;
using Yarde.EventDispatcher;

namespace Yarde.Code.Flows
{
    public abstract class BaseFlow
    {
        private readonly IDispatcher _dispatcher;
        private IListener _listener;

        protected BaseFlow(IDispatcher dispatcher) => _dispatcher = dispatcher;

        public async UniTask Start(IEvent startingEvent = null)
        {
            _listener = new EventListener(GetType().Name);
            await OnStart(_listener);
            await _dispatcher.AddListener(_listener, startingEvent);
        }

        public async UniTask Cancel()
        {
            await OnCancel(_listener);
            _dispatcher.RemoveListener(_listener);
        }

        protected abstract UniTask OnStart(IListener listener);
        protected abstract UniTask OnCancel(IListener listener);
    }
}
