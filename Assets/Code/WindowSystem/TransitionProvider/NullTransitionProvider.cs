using Cysharp.Threading.Tasks;

namespace Yarde.WindowSystem.TransitionProvider
{
    public sealed class NullTransitionProvider : ITransitionProvider
    {
        public void Init() { }
        public async UniTask TransitionIn() => await UniTask.CompletedTask;
        public async UniTask TransitionOut() => await UniTask.CompletedTask;
    }
}
