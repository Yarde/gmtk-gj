using System;
using Cysharp.Threading.Tasks;
using Yarde.Utils.Logger;

namespace Yarde.EventDispatcher
{
    [LogSettings(color:"#FAA")]
    public interface IListener
    {
        string Name { get; }
        IEvent CurrentEvent { get; }
        void AddHandler<T>(Func<T, UniTask> handler) where T : IEvent;
        bool CanHandleEvent(IEvent ev);
        UniTask Start();
        void Cancel();
    }
}
