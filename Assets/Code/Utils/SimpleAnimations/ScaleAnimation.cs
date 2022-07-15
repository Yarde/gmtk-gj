using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde.Utils.SimpleAnimations
{
    [RequireComponent(typeof(RectTransform))]
    public class ScaleAnimation : SimpleAnimationBase
    {
        private const string ANIMATION_ID = "SimpleAnimations::ScaleAnimation";

        [SerializeField] private float scale = 1.2f;
        [SerializeField] private float delay = 5.0f;
        [SerializeField] private float duration = 0.5f;

        private Transform _transform;

        protected override void Awake()
        {
            _transform = GetComponent<RectTransform>();

            base.Awake();
        }

        protected override async UniTask PlayEffect()
        {
            await DOTween.Sequence().SetId(ANIMATION_ID)
                .Append(_transform.DOScale(scale, duration).SetLoops(2, LoopType.Yoyo));

            await UniTask.Delay(Math.Max(delay - duration * 2, 0).ToMilliseconds());
        }
    }
}
