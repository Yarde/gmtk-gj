using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.UI
{
    public class MenuWindow : WindowBase
    {
        [SerializeField] private TextMeshProUGUI text;

        private const string TEXT_FORMAT = "Points: {0}\nTime left: {1}";
        
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
            text.text = string.Format(TEXT_FORMAT, player.Points, player.HealthPoints);
        }
    }
}
