using DG.Tweening;
using UnityEngine;

namespace Yarde.WindowSystem.TransitionProvider
{
    public partial class TransitionProvider
    {
        private sealed class DoTweenScaleTransition : DoTweenTransition
        {
            private readonly Vector3 _hiddenScale = Vector3.zero;
            private readonly Vector3 _shownScale;
            
            public DoTweenScaleTransition(TransitionProvider target) : base(target)
            {
                Transform targetTransform = target.transform;
                _shownScale = targetTransform.localScale;
                targetTransform.localScale = _hiddenScale;
            }

            protected override Tween CreateTween(TransitionProvider target)
            {
                return target.transform.DOScale(_shownScale, target.duration).SetEase(target.ease).SetDelay(target.delay);
            }
        }
    }
}
