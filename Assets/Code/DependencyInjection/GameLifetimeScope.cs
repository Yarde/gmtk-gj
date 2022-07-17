using UnityEngine;
using VContainer;
using VContainer.Unity;
using Yarde.Code.Flows;
using Yarde.EventDispatcher;
using Yarde.WindowSystem;
using Yarde.WindowSystem.BlendProvider;
using Yarde.WindowSystem.CanvasProvider;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.DependencyInjection
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private BlendViewBase blendViewBase;
        [SerializeField] private CanvasManagerBase canvasManagerBase;
        [SerializeField] private InputManager inputManager;
        private Player _player;

        protected override void Configure(IContainerBuilder builder)
        {
            _player = FindObjectOfType<Player>();
            
            RegisterSystems(builder);
            RegisterFlows(builder);
            RegisterComponents(builder);
        }

        private static void RegisterSystems(IContainerBuilder builder)
        {
            builder.Register<IWindowManager, DictionaryWindowManager>(Lifetime.Scoped);
            builder.Register<IWindowProvider, ScriptableObjectWindowProvider>(Lifetime.Scoped);
            builder.Register<IDispatcher, ListDispatcher>(Lifetime.Scoped);
        }

        private static void RegisterFlows(IContainerBuilder builder)
        {
            builder.Register<RootFlowBase, RootFlow>(Lifetime.Scoped);
            builder.Register<MenuFlowBase, MenuFlow>(Lifetime.Scoped);
        }

        private void RegisterComponents(IContainerBuilder builder)
        {
            builder.RegisterComponent(blendViewBase);
            builder.RegisterComponent(canvasManagerBase);
            builder.RegisterComponent(inputManager);
            builder.RegisterComponent(_player);
        }
    }
}
