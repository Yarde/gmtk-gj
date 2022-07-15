using Cysharp.Threading.Tasks;

namespace Yarde.WindowSystem.BlendProvider
{
    public sealed class NullBlendProvider : IBlendProvider
    {
        public async UniTask Enter() => await UniTask.CompletedTask;

        public async UniTask Exit() => await UniTask.CompletedTask;
    }
}
