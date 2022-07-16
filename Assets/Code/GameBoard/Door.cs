using Cysharp.Threading.Tasks;
using VContainer;

namespace Yarde.GameBoard
{
    public class Door :ObstacleBase
    {
        [Inject] private Player _player;
        // public override  UniTask OnTouch()
        // {
        //     if (_player.TopSide < Hp) return;
        //     Destroy(gameObject);
        //     return UniTask.CompletedTask;
        // }
        public override UniTask OnTouch()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }
}