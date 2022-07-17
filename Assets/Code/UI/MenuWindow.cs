using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.UI
{
    public class MenuWindow : WindowBase
    {
        [SerializeField] private Image clock;
        
        public async UniTask Setup(Player player)
        {
            this.LogVerbose("Setup");
            player.OnDamage += OnDamage;
            await UniTask.CompletedTask;
        }

        private async void OnDamage(float damage)
        {
            await clock.transform.DOScale(Vector3.one * (1 + damage / 6), 0.1f);
            await clock.transform.DOScale(Vector3.one, 0.1f);
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
