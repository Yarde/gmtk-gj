using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.Utils.Extensions;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.BlendProvider;
using Yarde.WindowSystem.TransitionProvider;

namespace Yarde.WindowSystem.WindowProvider
{
    [LogSettings(color:"#282")]
    public abstract class WindowBase : MonoBehaviour
    {
        public event Action OnEnter;
        public event Action OnExit;
        
        private IBlendProvider _blendProvider;
        private IBlendProvider BlendProvider => _blendProvider ??= gameObject.GetProvider<IBlendProvider>(new NullBlendProvider());
        
        private ITransitionProvider[] _transitionsProviders;


        internal virtual async UniTask Enter()
        {
            await UniTask.WhenAll(BlendProvider.Enter(), EnterElementTransitions());
            
            OnEnter?.Invoke();
        }

        internal virtual async UniTask Exit()
        {
            await UniTask.WhenAll(BlendProvider.Exit(), ExitElementTransitions());
            
            OnExit?.Invoke();
        }
        
        private void SetupElementTransitions()
        {
            if (_transitionsProviders == null)
            {
                _transitionsProviders = GetComponentsInChildren<ITransitionProvider>();
                foreach (ITransitionProvider transition in _transitionsProviders)
                {
                    transition.Init();
                }
            }
        }

        private async UniTask EnterElementTransitions()
        {
            SetupElementTransitions();
            await UniTask.WhenAll(_transitionsProviders.Select(e => e.TransitionInSafe()));
        }

        private async UniTask ExitElementTransitions()
        {
            SetupElementTransitions();
            await UniTask.WhenAll(_transitionsProviders.Select(e => e.TransitionOutSafe()));
        }
    }
}
