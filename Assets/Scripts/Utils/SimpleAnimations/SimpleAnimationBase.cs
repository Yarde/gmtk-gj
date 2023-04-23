using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde.Utils.SimpleAnimations
{
    public abstract class SimpleAnimationBase : MonoBehaviour
    {
        private CancellationTokenSource _cancellationToken;
        [SerializeField] private float initialDelay;

        protected virtual void Awake()
        {
            _cancellationToken = new CancellationTokenSource();

            Animate().Forget();
        }

        private void OnDestroy()
        {
            if (_cancellationToken != null)
            {
                _cancellationToken.Cancel();
                _cancellationToken.Dispose();
                _cancellationToken = null;
            }
        }

        private async UniTask Animate()
        {
            await UniTask.Delay(initialDelay.ToMilliseconds());
            while (_cancellationToken != null && !_cancellationToken.IsCancellationRequested)
            {
                await PlayEffect();
            }
        }

        protected abstract UniTask PlayEffect();
    }
}