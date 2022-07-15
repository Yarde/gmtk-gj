using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Yarde.Utils.SimpleAnimations
{
    [RequireComponent(typeof(Slider))]
    public class InfiniteSliderAnimation : SimpleAnimationBase
    {
        public enum InfiniteSliderType
        {
            LeftToRight,
            PingPong
        }
        
        [SerializeField] private InfiniteSliderType type = InfiniteSliderType.LeftToRight;
        [SerializeField] private float duration = 3.0f;
        [SerializeField] private Ease ease = Ease.Linear;
        
        private Slider _slider;

        protected override void Awake()
        {
            _slider = GetComponent<Slider>();
            _slider.interactable = false;
            base.Awake();
        }

        protected override async UniTask PlayEffect()
        {
            await _slider.DOValue(1f, duration).SetEase(ease);

            switch(type)
            {
                case InfiniteSliderType.LeftToRight:
                    _slider.value = 0f;
                    break;
                case InfiniteSliderType.PingPong:
                    await _slider.DOValue(0f, duration).SetEase(ease);
                    break;
            }
        }
    }
}

