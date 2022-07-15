using UnityEngine;
using VContainer;
using Yarde.Utils.Logger;

namespace Yarde.WindowSystem.BlendProvider
{
    public class BlendComponent : MonoBehaviour
    {
        [Header("Blend Settings")]
        [Tooltip("Configures screen cover that appears under the whole view of the window/popup")]
        [SerializeField] private BlendType blendType;

        private BlendViewBase _blendView;

        [Inject] 
        private void Construct(BlendViewBase blendView)
        {
            TryGetBlend(blendView, gameObject.transform);
        }

        private void TryGetBlend(BlendViewBase blends, Transform parent)
        {
            if (blends == null)
            {
                this.LogError("BlendViewBase prefab not available. Will not support any blends.");
                return;
            }

            blends.GetBlend(blendType, parent);
        }
    }
}