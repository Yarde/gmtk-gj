using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using VContainer;
using Yarde.EventDispatcher;

namespace Yarde.Code.Flows
{
    public abstract class RootFlowBase : BaseFlow
    {
        protected RootFlowBase(IDispatcher dispatcher) : base(dispatcher) { }
    }
    
    public class RootFlow : RootFlowBase
    {
        [Inject] [UsedImplicitly] private MenuFlowBase _menu;
        [Inject] [UsedImplicitly] private IDispatcher _dispatcher;
        
        public RootFlow(IDispatcher dispatcher) : base(dispatcher) { }
        protected override async UniTask OnStart(IListener listener)
        {
            listener.AddHandler<MenuOpenEvent>(OpenMenu);

            await UniTask.CompletedTask;
        }

        private async UniTask OpenMenu(MenuOpenEvent ev)
        {
            await _menu.Start(ev);
        }

        protected override async UniTask OnCancel(IListener listener)
        { 
            await _menu.Cancel();
        }
    }
}
