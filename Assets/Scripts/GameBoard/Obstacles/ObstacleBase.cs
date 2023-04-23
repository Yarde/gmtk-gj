using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class ObstacleBase : BoardObject
    {
        [SerializeField] private float hp;
        protected float Hp => hp;
        public abstract bool OnTouch();
    }
}