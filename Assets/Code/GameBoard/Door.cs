using Cysharp.Threading.Tasks;
using VContainer;

namespace Yarde.GameBoard
{
    public class Door : ObstacleBase
    {
        [Inject] private Player _player;

        public override UniTask OnTouch()
        {
            if (_player.TopSide >= Hp)
                Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }
}