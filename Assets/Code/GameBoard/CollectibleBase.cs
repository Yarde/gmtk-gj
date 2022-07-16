using System;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class CollectibleBase : BoardObject
    {
        public abstract CollectibleReward Collect();
        
    }
    
    [Serializable]
    public class CollectibleReward
    {
        public float hearts;
        public float points;
        public float speed;
    }
}
