using System;
using UnityEngine;
using Yarde.Utils.SerializableDictionary;

namespace Yarde.WindowSystem.WindowProvider
{
    [CreateAssetMenu(fileName = "WindowConfig", menuName = "Tools/WindowConfig")]
    public class WindowConfig : ScriptableObject
    {
        public WindowTypeToPrefab windows;
    }
    
    [Serializable] public class WindowTypeToPrefab : SerializableDictionary<WindowType, WindowBase> {}
}
