using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class EnemyBase : BoardObject
    {
        [SerializeField] private float hp;
        [SerializeField] private float damage;

        public float Hp => hp;
        public float Damage => damage;
        public abstract Vector3 GetEnemyMove();
        public abstract UniTask Kill();
        public abstract UniTask MakeMove(Vector3 direction);
        public abstract UniTask MakeHalfMove(Vector3 direction);

    }
}
