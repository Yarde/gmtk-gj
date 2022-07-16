using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer;
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
        [Inject] [UsedImplicitly] private Player _player;
        
        private readonly IWindowManager _windowManager;
        private MenuWindow _window;

        public MenuFlow(IDispatcher dispatcher, IWindowManager windowManager) : base(dispatcher) => _windowManager = windowManager;

        protected override async UniTask OnStart(IListener listener)
        {
            listener.AddHandler<MenuOpenEvent>(OnMenuOpen);
            listener.AddHandler<BackButtonEvent>(_ => Cancel());
            _player.OnUpdate += PlayerOnUpdate;
            await UniTask.CompletedTask;
        }

        private void PlayerOnUpdate() {
             _window.UpdateWindow(_player);
        }

        protected override async UniTask OnCancel(IListener listener)
        {
            await OnMenuClose();
        }

        private async UniTask OnMenuOpen(MenuOpenEvent ev)
        {
            _window = await _windowManager.Add<MenuWindow>(WindowType.Menu, async window => await window.Setup());
        }

        private async UniTask OnMenuClose()
        {
            await _windowManager.Remove(WindowType.Menu);
        }
    }

}
