using Cysharp.Threading.Tasks;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.UI
{
    public class MenuWindow : WindowBase
    {
        public async UniTask Setup()
        {
            this.LogVerbose("Setup");
            await UniTask.CompletedTask;
        }

        internal override async UniTask Enter() 
        {
            await base.Enter();
            this.LogVerbose("OnEnter");         
        }

        internal override async UniTask Exit() 
        {
            await base.Exit();
            this.LogVerbose("OnExit");
        }
    }
}
