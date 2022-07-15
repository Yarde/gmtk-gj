using System;
using UnityEngine;
using Yarde.Utils.Logger;
using Yarde.Utils.SerializableDictionary;

namespace Yarde.WindowSystem.BlendProvider
{
    [Serializable] public class BlendProviderToType : SerializableDictionary<BlendType, BlendProviderBase> {}
    
    public class BlendView : BlendViewBase
    {
        [SerializeField] private BlendProviderToType blends;

        internal override void GetBlend(BlendType type, Transform parent)
        {
            if (blends.TryGetValue(type, out BlendProviderBase provider))
            {
                BlendProviderBase blend = Instantiate(provider, parent, false);
                blend.transform.SetAsFirstSibling();
                return;
            }

            this.LogError($"WindowBlend of type {type} not supported. Configure it in the {name} prefab.");
        }
    }
}
