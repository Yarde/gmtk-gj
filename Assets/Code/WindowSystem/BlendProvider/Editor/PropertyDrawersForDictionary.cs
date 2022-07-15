using UnityEditor;
using Yarde.Utils.SerializableDictionary.Editor;

namespace Yarde.WindowSystem.BlendProvider.Editor
{
    [CustomPropertyDrawer(typeof(BlendProviderToType))] 
    public class BlendProviderToTypePropertyDrawer : SerializableDictionaryPropertyDrawer {}
}
