using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Yarde.GameBoard;

namespace Yarde
{
    public class EnemyFollowingPlayer : EnemyBase
    {
        [SerializeField] private float moveDelayInSec = 1;
        [SerializeField] private float movementSpeed = 1;
        [SerializeField] private float distanceToActivate = 5f;
        [Inject] private Player _player;
        private Vector3 _targetPosition;
        private int _turn;
        private int _waypointIndex;

        public override Vector3 GetEnemyMove()
        {
            _turn++;
            if (_turn % turnsToMove - 1 == 0)
            {
                _targetPosition = _player.transform.position;
                Vector3 delta = _targetPosition - transform.position;
                if (delta.magnitude > distanceToActivate)
                {
                    return Vector3.zero;
                }
                Vector3 perpendicularTargetPosition = _targetPosition;
                if (Mathf.Abs(delta.x) - Mathf.Abs(delta.z) > 0)
                {
                    perpendicularTargetPosition.z = transform.position.z;
                }
                else
                {
                    perpendicularTargetPosition.x = transform.position.x;
                }
                return Vector3.MoveTowards(transform.position, perpendicularTargetPosition, movementSpeed);
            }

            return Vector3.zero;
        }

        public override async UniTask Kill()
        {
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }

        public override async UniTask MakeMove(Vector3 direction)
        {
            if (gameObject == null && !ReferenceEquals(gameObject, null))
            {
                return;
            }
            await transform.DOMove(direction, moveDelayInSec);
        }

        public override async UniTask MakeHalfMove(Vector3 direction)
        {
            if (gameObject == null && !ReferenceEquals(gameObject, null))
            {
                return;
            }
            Vector3 prevPosition = transform.position;
            Vector3 newDirection = Vector3.MoveTowards(prevPosition, direction, 0.5f);
            await transform.DOMove(newDirection, moveDelayInSec / 2);
            if (gameObject == null && !ReferenceEquals(gameObject, null))
            {
                return;
            }
            await transform.DOMove(prevPosition, moveDelayInSec / 2);
        }
    }
}
