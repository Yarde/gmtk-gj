using UnityEngine;

namespace Yarde.WindowSystem.BlendProvider
{
    public abstract class BlendViewBase : MonoBehaviour
    {
        internal abstract void GetBlend(BlendType type, Transform parent);
    }
}
