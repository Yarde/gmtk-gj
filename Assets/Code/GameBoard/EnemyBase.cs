using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class EnemyBase : BoardObject
    {
        public abstract float Hp { get; }
        public abstract float Damage { get; }
        public abstract Vector3 GetEnemyMove();
        public abstract UniTask Kill();
        public abstract UniTask MakeMove(Vector3 direction);
        public abstract UniTask MakeHalfMove(Vector3 direction);

    }
}
