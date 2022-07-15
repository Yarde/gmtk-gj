using Cysharp.Threading.Tasks;

namespace Yarde.WindowSystem.TransitionProvider
{
    public interface ITransitionProvider
    {
        void Init();
        UniTask TransitionIn();
        UniTask TransitionOut();
    }

}
