using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class ObstacleBase : BoardObject
    {
        [SerializeField] private float hp;
        public float Hp => hp;
        public abstract UniTask OnTouch();
    }
}
