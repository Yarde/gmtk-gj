using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.UI
{
    public class MenuWindow : WindowBase
    {
        [SerializeField] private Image clock;
        
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

        public void UpdateWindow(Player player)
        {
            var time = player.HealthPoints / player.MaxHealthPoints;
            clock.fillAmount = time;
        }
    }
}
