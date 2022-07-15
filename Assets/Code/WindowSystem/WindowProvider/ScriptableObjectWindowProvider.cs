using UnityEngine;
using Yarde.Utils.Logger;

namespace Yarde.WindowSystem.WindowProvider
{
    internal class ScriptableObjectWindowProvider : IWindowProvider
    {
        private WindowConfig _config;
        
        public ScriptableObjectWindowProvider()
        {
            _config = Resources.Load<WindowConfig>("WindowConfig"); 
        }
        
        public T GetWindow<T>(WindowType windowType) where T : WindowBase
        {
            if (_config == null)
            {
                this.LogError("Missing Window Config file");
                return null;
            }
            
            if (_config.windows.TryGetValue(windowType, out WindowBase window))
            {
                return window as T;
            }
            this.LogError($"Failed to get window with type {windowType}");
            return null;
        }
    }
}
