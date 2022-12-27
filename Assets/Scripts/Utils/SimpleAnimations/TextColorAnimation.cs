using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde.Utils.SimpleAnimations
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextColorAnimation : SimpleAnimationBase
    {
        [SerializeField] private Color colorStart = Color.white;
        [SerializeField] private Color colorEnd = Color.black;
        [SerializeField] private float delay = 5.0f;
        [SerializeField] private float duration = 0.5f;

        private TMP_Text _textComponent;

        protected override void Awake()
        {
            _textComponent = GetComponent<TMP_Text>();
            base.Awake();
        }

        protected override async UniTask PlayEffect()
        {
            await _textComponent.DOColor(colorEnd, duration);
            await _textComponent.DOColor(colorStart, duration);
            
            await UniTask.Delay(Mathf.Max(delay - duration * 2, 0f).ToMilliseconds());
        }
    }
}
