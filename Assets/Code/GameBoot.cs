using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using Yarde.Code.Flows;
using Yarde.Utils.Logger;
using Logger = Yarde.Utils.Logger.Logger;

namespace Yarde
{
    public class GameBoot : MonoBehaviour
    {
        [Inject] [UsedImplicitly] private RootFlowBase _rootFlow;
        
        [SerializeField] private LoggerLevel loggerLevel;

        private async void Start()
        {
            Logger.Level = loggerLevel;
            
            await _rootFlow.Start(new MenuOpenEvent()).SuppressCancellationThrow();
        }
    }
}
