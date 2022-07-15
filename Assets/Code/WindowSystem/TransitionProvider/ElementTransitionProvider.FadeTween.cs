using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yarde.Utils.Logger;

namespace Yarde.WindowSystem.TransitionProvider
{
    public partial class TransitionProvider
    {
        private sealed class DoTweenFadeTransition : DoTweenTransition
        {
            private readonly Graphic _graphic;
            private readonly CanvasGroup _group;

            private const float HIDDEN_ALPHA = 0.0f;
            private readonly float _shownAlpha;
            
            public DoTweenFadeTransition(TransitionProvider target) : base(target)
            {
                if (target.TryGetComponent(out _graphic))
                {
                    _shownAlpha = _graphic.color.a;
                    SetColorAlpha(_graphic, HIDDEN_ALPHA);
                }
                else if (target.TryGetComponent(out _group))
                {
                    _shownAlpha = _group.alpha;
                    SetColorAlpha(_group, HIDDEN_ALPHA);
                }
            }

            protected override Tween CreateTween(TransitionProvider target)
            {
                if (_graphic)
                {
                    return _graphic.DOFade(_shownAlpha, target.duration).SetEase(target.ease).SetDelay(target.delay);
                }
                if (_group)
                {
                    return _group.DOFade(_shownAlpha, target.duration).SetEase(target.ease).SetDelay(target.delay);
                }


                this.LogError($"Target {target.name} doesn't have a {nameof(Graphic)} nor {nameof(CanvasGroup)} component.");
                return null;
            }

            private void SetColorAlpha(Graphic graphic, float alpha)
            {
                Color color = graphic.color;
                color.a = alpha;
                graphic.color = color;
            }
            private void SetColorAlpha(CanvasGroup group, float alpha)
            {
                group.alpha = alpha;
            }
        }
    }
}
