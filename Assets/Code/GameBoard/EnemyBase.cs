using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class EnemyBase : BoardObject
    {
        public float Damage { get; set; }
        public abstract Vector3 GetEnemyMove();
        public abstract bool CheckPlayerHit(Vector3 direction);
        public abstract UniTask Kill();
        public abstract UniTask MakeMove(Vector3 direction);
        public abstract UniTask MakeHalfMove(Vector3 direction);

    }
}
