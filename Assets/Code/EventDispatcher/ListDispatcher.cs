using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Yarde.EventDispatcher
{
    public class ListDispatcher : IDispatcher
    {
        private readonly List<IListener> _listeners = new List<IListener>();

        public IReadOnlyList<IListener> ActiveListeners => _listeners;

        public async UniTask AddListener(IListener listener, IEvent startingEvent)
        {
            _listeners.Add(listener);
            UniTask flowTask = listener.Start();
            if (startingEvent != null)
            {
                RaiseEvent(startingEvent);
            }
            await flowTask;
        }

        public void RemoveListener(IListener listener)
        {
            listener.Cancel();
            _listeners.Remove(listener);
        }

        public bool RaiseEvent(IEvent ev)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                IListener listener = _listeners[i];
                if (listener.CanHandleEvent(ev))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
