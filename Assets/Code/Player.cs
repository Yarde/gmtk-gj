using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.GameBoard;
using Yarde.Utils.Logger;

namespace Yarde
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float angleIncrement = 5;
        [SerializeField] private int moveDelayInMillis = 10;
        [SerializeField] private float startingHealth = 3;

        private float _healthPoints;
        public Vector2 Size => new Vector2(1, 1);

        private void Awake()
        {
            _healthPoints = startingHealth;
        }

        public async UniTask Roll(Vector3 direction)
        {
            Vector3 anchor = transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 0; i < 90 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
        }

        public async UniTask HalfRoll(Vector3 direction)
        {
            Vector3 anchor = transform.position + (Vector3.down + direction) * 0.5f;
            Vector3 axis = Vector3.Cross(Vector3.up, direction);

            for (int i = 0; i < 45 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
            
            for (int i = 0; i < 45 / angleIncrement; i++)
            {
                transform.RotateAround(anchor, axis, -angleIncrement);
                await UniTask.Delay(moveDelayInMillis);
            }
        }

        public void TakeDamage(float damage)
        {
            _healthPoints -= damage;

            if (_healthPoints <= 0)
            {
                this.LogError("Game Lost!");
            }
        }

        public async UniTask OnEnemyKilled(EnemyBase kill)
        {
            // todo add some points of other shit
        }

        public async UniTask CollectItem(CollectibleReward collect)
        {
            // todo apply collected reward
        }
    }
}
