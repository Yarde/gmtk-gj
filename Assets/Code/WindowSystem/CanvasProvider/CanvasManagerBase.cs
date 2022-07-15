using UnityEngine;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.WindowSystem.CanvasProvider
{
    [LogSettings(color:"#aaf")]
    internal abstract class CanvasManagerBase : MonoBehaviour
    {
        public abstract Transform GetWindowParent();
        public abstract void EnableRaycasts(WindowType unblockingType);
        public abstract void DisableRaycasts(WindowType blockingType);
    }
}
