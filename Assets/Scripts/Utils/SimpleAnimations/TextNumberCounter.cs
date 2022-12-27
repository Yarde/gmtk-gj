using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Yarde.Utils.Extensions;

namespace Yarde.Utils.SimpleAnimations
{
    [RequireComponent(typeof(TMP_Text))]
    public class TextNumberCounter : MonoBehaviour
    {
        private const float EPS = 0.01f;
        
        [SerializeField] private string textFormat = "{0}";
        [SerializeField] private float counterDuration = 0.5f;
        [SerializeField] private Ease counterEase = Ease.OutQuad;
        [SerializeField] private bool abbreviateText = true;

        private TMP_Text _target;

        private long? _targetValue;
        private long? _valueShown;
        private Tween _valueShownTween;

        public TMP_Text Target
        {
            get
            {
                if (_target == null)
                {
                    _target = GetComponent<TMP_Text>();
                }

                return _target;
            }
        }

        private long ValueShown
        {
            get => _valueShown ?? 0;
            set
            {
                if (_valueShown != value)
                {
                    _valueShown = value;
                    Target.text = abbreviateText ? string.Format(textFormat, _valueShown.Value.AbbreviateNumber()) : string.Format(textFormat, _valueShown);
                }
            }
        }
        
        private void Awake()
        {
            if (_target == null)
            {
                _target = GetComponent<TMP_Text>();
            }
        }

        public void InitNewValue(long newValue) => SetNewValue(newValue, 0.0f).Forget();
        public void SetNewValue(long newValue) => SetNewValue(newValue, counterDuration, counterEase).Forget();
        
        public async UniTask SetNewValue(long newValue, float duration, Ease ease = Ease.Linear)
        {
            if (_targetValue != newValue)
            {
                if (_valueShownTween != null && _valueShownTween.IsActive())
                {
                    _valueShownTween.Kill();
                }
                
                _targetValue = newValue;
                if (duration < EPS)
                {
                    ValueShown = _targetValue.Value;
                }
                else
                {
                    await (_valueShownTween = DOTween.To(() => ValueShown, x => ValueShown = x, _targetValue.Value, duration).SetEase(ease));
                }
            }
        }
    }
}
