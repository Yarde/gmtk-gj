using System.Collections.Generic;
using UnityEngine;
using Yarde.Utils.Extensions;
using Yarde.Utils.Logger;
using Yarde.WindowSystem.WindowProvider;

namespace Yarde.WindowSystem.CanvasProvider
{
    internal class SimpleCanvasManager : CanvasManagerBase
    {
        [SerializeField] private CanvasComponent mainCanvas;

        private readonly Dictionary<WindowType, int> _typeToBlockadeCounter = new Dictionary<WindowType, int>();

        public override Transform GetWindowParent() => mainCanvas.WindowParent;

        public override void EnableRaycasts(WindowType unblockingType)
        {
            if (_typeToBlockadeCounter.ContainsKey(unblockingType))
            {
                if (_typeToBlockadeCounter[unblockingType] > 1)
                {
                    _typeToBlockadeCounter[unblockingType]--;
                }
                else
                {
                    _typeToBlockadeCounter.Remove(unblockingType);
                }
            }
            else
            {
                return;
            }

            mainCanvas.GraphicRaycaster.enabled = _typeToBlockadeCounter.Count == 0;
            if (_typeToBlockadeCounter.Count == 0)
            {
                this.LogVerbose($"[CanvasManager] removed block on {unblockingType}, Raycast in now active");
            }
            else
            {
                this.LogVerbose($"[CanvasManager] removed block on {unblockingType}, Still blocking elements:\n{_typeToBlockadeCounter.ContentToString()}");
            }
        }

        public override void DisableRaycasts(WindowType blockingType)
        {
            if (_typeToBlockadeCounter.ContainsKey(blockingType))
            {
                _typeToBlockadeCounter[blockingType]++;
            }
            else
            {
                _typeToBlockadeCounter[blockingType] = 1;
            }

            mainCanvas.GraphicRaycaster.enabled = false;
            this.LogVerbose($"[CanvasManager] added block on {blockingType}, Total of blocking elements {_typeToBlockadeCounter.Count}");
        }
    }
}
