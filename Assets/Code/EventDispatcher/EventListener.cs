using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Yarde.Utils.Logger;

namespace Yarde.EventDispatcher
{
    public class EventListener : IListener
    {
        private readonly Dictionary<Type, Func<IEvent, UniTask>> _handlers = new Dictionary<Type, Func<IEvent, UniTask>>();
        private CancellationTokenSource _cancellationToken;
        private UniTaskCompletionSource<IEvent> _nextEventAwaiter;
        private IEvent _currentEvent;

        private readonly string _name;

        public EventListener(string name) => _name = name;

        public string Name => _name;
        public IEvent CurrentEvent => _currentEvent;

        public void AddHandler<T>(Func<T, UniTask> handler) where T : IEvent
        {
            _handlers[typeof(T)] = evt => handler((T)evt);
        }

        public bool CanHandleEvent(IEvent ev)
        {
            if (_cancellationToken.IsCancellationRequested)
            {
                return false;
            }

            if (GetHandler(ev.GetType()) == null)
            {
                return false;
            }

            if (_nextEventAwaiter.Task.Status != UniTaskStatus.Pending)
            {
                string currentEventName = _currentEvent != null ? _currentEvent.GetType().ToString() : "null";
                this.LogVerbose($"Loop {_name} is busy handling {currentEventName}");
                return false;
            }

            _nextEventAwaiter.TrySetResult(ev);
            return true;
        }

        private Func<IEvent, UniTask> GetHandler(Type t)
        {
            while (t != null)
            {
                if (_handlers.ContainsKey(t))
                {
                    return _handlers[t];
                }

                t = t.BaseType;
            }

            return null;
        }

        private async UniTask<IEvent> WaitForNextEvent(CancellationToken cancellationToken)
        {
            _nextEventAwaiter = new UniTaskCompletionSource<IEvent>();
            using (cancellationToken.Register(() =>
            {
                {
                    _nextEventAwaiter.TrySetCanceled();
                }
            }))
            {
                try
                {
                    await _nextEventAwaiter.Task;
                }
                catch (OperationCanceledException)
                {
                    this.LogVerbose($"Loop {_name} canceled");
                }
            }

            if (_nextEventAwaiter.Task.Status == UniTaskStatus.Succeeded)
            {
                IEvent result = _nextEventAwaiter.Task.GetAwaiter().GetResult();
                return result;
            }
            else
            {
                return null;
            }
        }

        public async UniTask Start()
        {
            _cancellationToken = new CancellationTokenSource();

            while (_cancellationToken is { IsCancellationRequested: false })
            {
                IEvent ev = await WaitForNextEvent(_cancellationToken.Token);

                if (_cancellationToken.IsCancellationRequested)
                {
                    break;
                }

                this.LogVerbose($"Begin handle {ev} in loop {_name} ");
                _currentEvent = ev;
                await GetHandler(ev.GetType())(ev);
                _currentEvent = null;
                this.LogVerbose($"End handle {ev} in loop {_name}");
            }

            this.LogVerbose($"Ending loop {_name}");
        }

        public void Cancel()
        {
            if (_cancellationToken is { IsCancellationRequested: false })
            {
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
                _cancellationToken = null;
            }
        }
    }
}
