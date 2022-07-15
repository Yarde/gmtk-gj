using Cysharp.Threading.Tasks;
using Yarde.EventDispatcher;
using Yarde.UI;
using Yarde.WindowSystem;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.Code.Flows
{
    public abstract class MenuFlowBase : BaseFlow
    {
        protected MenuFlowBase(IDispatcher dispatcher) : base(dispatcher) { }
    }
    
    public class MenuFlow : MenuFlowBase
    {
        private readonly IWindowManager _windowManager;

        public MenuFlow(IDispatcher dispatcher, IWindowManager windowManager) : base(dispatcher) => _windowManager = windowManager;

        protected override async UniTask OnStart(IListener listener)
        {
            listener.AddHandler<MenuOpenEvent>(OnMenuOpen);
            listener.AddHandler<BackButtonEvent>(_ => Cancel());
            await UniTask.CompletedTask;
        }

        protected override async UniTask OnCancel(IListener listener)
        {
            await OnMenuClose();
        }

        private async UniTask OnMenuOpen(MenuOpenEvent ev)
        {
            await _windowManager.Add<MenuWindow>(WindowType.Menu, async window => await window.Setup());
        }

        private async UniTask OnMenuClose()
        {
            await _windowManager.Remove(WindowType.Menu);
        }
    }

    public sealed class MenuOpenEvent : IEvent { }
}
