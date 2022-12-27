using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde.Utils.SimpleAnimations
{
    [RequireComponent(typeof(RectTransform))]
    public class JumpAnimation : SimpleAnimationBase
    {
        [SerializeField] private float moveDistance = 5.0f;
        [SerializeField] private float delay = 5.0f;
        [SerializeField] private float duration = 0.5f;
        [SerializeField] private Ease easeIn = Ease.OutCubic;
        [SerializeField] private Ease easeOut = Ease.InCubic;

        private Transform _transform;
        
        protected override void Awake()
        {
            _transform = GetComponent<RectTransform>();
            
            base.Awake();
        }

        protected override async UniTask PlayEffect()
        {
            float positionY = _transform.localPosition.y;

            await _transform.DOLocalMoveY(positionY + moveDistance, duration).SetEase(easeIn);
            await _transform.DOLocalMoveY(positionY, duration).SetEase(easeOut);

            await UniTask.Delay(Mathf.Max(delay - duration * 2, 0f).ToMilliseconds());
        }
    }
}
