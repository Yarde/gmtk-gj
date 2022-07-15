using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Yarde.Utils.Logger;

namespace Yarde.WindowSystem.TransitionProvider
{
    public partial class TransitionProvider : MonoBehaviour, ITransitionProvider
    {
        private enum State
        {
            None,
            TransitionIn,
            In,
            TransitionOut,
            Out
        };

        private interface ITransition
        {
            State State { get; }

            UniTask TransitionIn(CancellationToken cancelToken);
            UniTask TransitionOut(CancellationToken cancelToken);

            void Kill();
        }

        private sealed class NullTransition : ITransition
        {
            public State State => State.None;

            public async UniTask TransitionIn(CancellationToken cancelToken) => await UniTask.CompletedTask;

            public async UniTask TransitionOut(CancellationToken cancelToken) => await UniTask.CompletedTask;

            public void Kill() { }
        }

        public enum AnimationType { TweenScale, TweenPosition, TweenAlpha }
        public enum MoveDirection { Top, Bottom, Left, Right }

        [SerializeField] private AnimationType animationType = AnimationType.TweenPosition;
        [SerializeField] [ShowIf(nameof(IsTweenPosition))] private MoveDirection moveDirection = MoveDirection.Bottom;
        [SerializeField] private float duration = 1.0f;
        [SerializeField] private float delay;
        [SerializeField] private Ease ease = Ease.Linear;

        private ITransition _transition;
        private CancellationTokenSource _cancelToken;

        private bool IsTweenPosition => animationType == AnimationType.TweenPosition;
        [ShowNativeProperty] [UsedImplicitly] private State TransitionState => _transition?.State ?? State.None;

        public MoveDirection Direction
        {
            get => moveDirection;
            set
            {
                if (animationType == AnimationType.TweenPosition && _transition == null)
                {
                    moveDirection = value;
                }
                else
                {
                    this.LogError("Cannot change Move Direction.");
                }
            }
        }

        public void Init()
        {
            if (_transition == null)
            {
                _transition = GetTransition(animationType);
            }
        }

        private void Awake()
        {
            _cancelToken = new CancellationTokenSource();
        }

        private void OnDestroy()
        {
            _cancelToken.Cancel();
            _cancelToken.Dispose();
            _cancelToken = null;

            _transition?.Kill();
        }

        public async UniTask TransitionIn()
        {
            if (_transition == null)
            {
                Init();
            }
            if (_cancelToken != null && !_cancelToken.Token.IsCancellationRequested && _transition != null)
            {
                await _transition.TransitionIn(_cancelToken.Token);
            }
        }

        public async UniTask TransitionOut()
        {
            if (_cancelToken != null && !_cancelToken.Token.IsCancellationRequested && _transition != null)
            {
                await _transition.TransitionOut(_cancelToken.Token);
            }
        }

        private ITransition GetTransition(AnimationType tweenType)
        {
            return tweenType switch
            {
                AnimationType.TweenPosition => new DoTweenMoveTransition(this),
                AnimationType.TweenScale => new DoTweenScaleTransition(this),
                AnimationType.TweenAlpha => new DoTweenFadeTransition(this),
                _ => new NullTransition()
            };
        }
    }
}
