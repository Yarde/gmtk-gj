using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yarde.Utils.Extensions;

namespace Yarde.WindowSystem.BlendProvider
{
    internal sealed class FadeBlendProvider : BlendProviderBase
    {
        [SerializeField] private Image fade;
        [SerializeField] private float duration;
        [SerializeField] private float alpha;

        public override async UniTask Enter()
        {
            await fade.DOColor(fade.color.WithA(alpha), duration);
        }

        public override async UniTask Exit()
        {
            await fade.DOColor(fade.color.WithA(0f), duration);
        }
    }
}
