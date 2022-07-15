using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.WindowSystem.BlendProvider
{
    public abstract class BlendProviderBase : MonoBehaviour, IBlendProvider
    {
        public virtual async UniTask Enter()
        {
            await UniTask.CompletedTask;
        }

        public virtual async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
    }
}

