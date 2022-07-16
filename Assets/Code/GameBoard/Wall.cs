using Cysharp.Threading.Tasks;

namespace Yarde.GameBoard
{
    public class Wall : ObstacleBase
    {
        public override async UniTask OnTouch()
        {
            await UniTask.CompletedTask;
        }
    }
}
