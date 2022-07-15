using Cysharp.Threading.Tasks;

namespace Yarde.WindowSystem.BlendProvider
{
    public interface IBlendProvider
    {
        UniTask Enter();
        UniTask Exit();
    }

}
