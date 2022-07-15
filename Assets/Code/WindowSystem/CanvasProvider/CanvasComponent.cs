using UnityEngine;
using UnityEngine.UI;

namespace Yarde.WindowSystem.CanvasProvider
{
    internal sealed class CanvasComponent : MonoBehaviour
    {
        [SerializeField] private Transform windowParent;
        
        public Canvas Canvas { get; private set; }
        public CanvasScaler CanvasScaler { get; private set; }
        public GraphicRaycaster GraphicRaycaster { get; private set; }
        public Transform WindowParent => windowParent;
        
        private void Awake()
        {
            Canvas = GetComponent<Canvas>();
            CanvasScaler = GetComponent<CanvasScaler>();
            GraphicRaycaster = GetComponent<GraphicRaycaster>();
        }
    }
}
