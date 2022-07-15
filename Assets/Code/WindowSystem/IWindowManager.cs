using System;
using Cysharp.Threading.Tasks;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.WindowSystem
{
    [LogSettings(color:"#ACA")]
    public interface IWindowManager
    {
        UniTask<T> Add<T>(WindowType windowType, Func<T, UniTask> setup = null) where T : WindowBase;
        UniTask Remove(WindowType windowType);
    }
}
