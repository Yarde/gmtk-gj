using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.CanvasProvider;
using Yarde.WindowSystem.WindowProvider;
using Object = UnityEngine.Object;

namespace Yarde.WindowSystem
{
    internal class DictionaryWindowManager : IWindowManager
    {
        [Inject] [UsedImplicitly] private IWindowProvider _windowProvider;
        [Inject] [UsedImplicitly] private CanvasManagerBase _canvasManager;

        private readonly WindowTypeToPrefab _windows = new WindowTypeToPrefab();

        private IObjectResolver _container;

        public DictionaryWindowManager(IObjectResolver container)
        {
            _container = container;
        }

        public async UniTask<T> Add<T>(WindowType windowType, Func<T, UniTask> setup = null) where T : WindowBase
        {
            if (_windows.ContainsKey(windowType))
            {
                this.LogError($"Window of type {windowType} already in dictionary!");
                return null;
            }
            
            T prefab = _windowProvider.GetWindow<T>(windowType);
            Transform parent = _canvasManager.GetWindowParent();
            T window = Object.Instantiate(prefab, parent);
            _container.InjectGameObject(window.gameObject);
            _windows.Add(windowType, window);

            _canvasManager.DisableRaycasts(windowType);
            if (setup != null)
            {
                await setup.Invoke(window);
            }

            await window.Enter();
            _canvasManager.EnableRaycasts(windowType);

            return window;
        }

        public async UniTask Remove(WindowType windowType)
        {
            if (_windows.TryGetValue(windowType, out WindowBase window))
            {
                _windows.Remove(windowType);

                _canvasManager.DisableRaycasts(windowType);
                await window.Exit();
                _canvasManager.EnableRaycasts(windowType);

                Object.Destroy(window.gameObject);
            }
            else
            {
                this.LogError($"Window for type {windowType} not available!");
            }
        }
    }
}
