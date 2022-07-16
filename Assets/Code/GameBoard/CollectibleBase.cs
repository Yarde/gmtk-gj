using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class CollectibleBase : MonoBehaviour
    {
        public abstract CollectibleReward Collect();
    }
    public class CollectibleReward
    {
        public float hearts;
        public float speed;
        // todo more rewards
    }
}
