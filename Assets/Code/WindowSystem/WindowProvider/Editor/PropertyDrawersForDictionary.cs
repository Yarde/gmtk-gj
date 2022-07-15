using UnityEditor;
using Yarde.Utils.SerializableDictionary.Editor;

namespace Yarde.WindowSystem.WindowProvider.Editor
{
    [CustomPropertyDrawer(typeof(WindowTypeToPrefab))] 
    public class WindowConfigPropertyDrawer : SerializableDictionaryPropertyDrawer {}
}
