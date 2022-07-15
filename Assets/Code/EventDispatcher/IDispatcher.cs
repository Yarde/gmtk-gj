using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Yarde.EventDispatcher
{
    public interface IDispatcher
    {
        IReadOnlyList<IListener> ActiveListeners { get; }
        UniTask AddListener(IListener listener, IEvent startingEvent);
        void RemoveListener(IListener listener);
        bool RaiseEvent(IEvent ev);
    }
}
