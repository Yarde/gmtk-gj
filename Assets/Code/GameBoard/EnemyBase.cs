using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class EnemyBase : MonoBehaviour
    {
        public abstract Vector3 GetEnemyMove();
        public abstract bool CheckPlayerHit(Vector3 direction);

        public abstract void MakeMove(Vector3 direction);
    }
}
