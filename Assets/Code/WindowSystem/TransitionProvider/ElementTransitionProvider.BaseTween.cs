using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Yarde.WindowSystem.TransitionProvider
{
    public partial class TransitionProvider
    {
        private abstract class DoTweenTransition : ITransition
        {
            private TransitionProvider target;

            private Tween _tween;

            public State State { get; private set; }

            protected DoTweenTransition(TransitionProvider target)
            {
                this.target = target;
            }

            public void Kill()
            {
                _tween?.Kill();
            }

            private void CreateTween()
            {
                _tween ??= CreateTween(target).SetAutoKill(false).OnComplete(OnTweenComplete).OnRewind(OnTweenRewind);
            }

            private void OnTweenComplete()
            {
                State = State.In;
            }
            private void OnTweenRewind()
            {
                State = State.Out;
            }

            public async UniTask TransitionIn(CancellationToken cancelToken)
            {
                if (State == State.In)
                {
                    return;
                }

                if (State != State.TransitionIn)
                {
                    CreateTween();
                    State = State.TransitionIn;
                    _tween.PlayForward();
                }

                await UniTask.WaitWhile(() => State != State.In, PlayerLoopTiming.Update, cancelToken);
            }

            public async UniTask TransitionOut(CancellationToken cancelToken)
            {
                if (State == State.Out)
                {
                    return;
                }

                if (State != State.TransitionOut)
                {
                    if (_tween == null)
                    {
                        CreateTween();
                        _tween.Complete();
                    }

                    State = State.TransitionOut;
                    _tween.PlayBackwards();
                }

                await UniTask.WaitWhile(() => State != State.Out, PlayerLoopTiming.Update, cancelToken);
            }

            protected abstract Tween CreateTween(TransitionProvider target);
        }
    }
}