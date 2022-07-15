using DG.Tweening;
using UnityEngine;
using Yarde.Utils.Extensions;
using Yarde.Utils.Logger;

namespace Yarde.WindowSystem.TransitionProvider
{
    public partial class TransitionProvider
    {
        private sealed class DoTweenMoveTransition : DoTweenTransition
        {
            private const float POSITION_OFFSET = 1000;

            private readonly RectTransform _transform;
            private readonly Vector2 _shownPosition;

            public DoTweenMoveTransition(TransitionProvider target) : base(target)
            {
                _transform = target.transform as RectTransform;
                if (_transform == null)
                {
                    this.LogError($"Missing RectTransform on {target.gameObject.FullPath()}");
                    return;
                }

                _shownPosition = _transform.anchoredPosition;
                Vector2 hiddenPosition = GetHiddenPosition(_shownPosition, target.moveDirection);

                _transform.anchoredPosition = hiddenPosition;
            }

            protected override Tween CreateTween(TransitionProvider target)
            {
                return _transform.DOAnchorPos(_shownPosition, target.duration).SetEase(target.ease).SetDelay(target.delay);
            }

            private Vector2 GetHiddenPosition(Vector2 shownPosition, MoveDirection direction)
            {
                // TODO: Instead of a const value, we should move the object just a bit outside of the screen
                switch (direction)
                {
                    case MoveDirection.Top:
                        shownPosition.y += POSITION_OFFSET;
                        break;
                    case MoveDirection.Bottom:
                        shownPosition.y -= POSITION_OFFSET;
                        break;
                    case MoveDirection.Left:
                        shownPosition.x -= POSITION_OFFSET;
                        break;
                    case MoveDirection.Right:
                        shownPosition.x += POSITION_OFFSET;
                        break;
                }

                return shownPosition;
            }
        }
    }
}