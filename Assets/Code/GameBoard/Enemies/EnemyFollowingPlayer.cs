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
        [Inject] private Player _player;
        private Vector3 _targetPosition;
        private int _waypointIndex;

        public override Vector3 GetEnemyMove()
        {
            _targetPosition = _player.transform.position;
            var delta = _targetPosition - transform.position;
            var perpendicularTargetPosition = _targetPosition;
            if (Mathf.Abs(delta.x) - Mathf.Abs(delta.z) > 0)
                perpendicularTargetPosition.z = transform.position.z;
            else
                perpendicularTargetPosition.x = transform.position.x;
            return Vector3.MoveTowards(transform.position, perpendicularTargetPosition, movementSpeed);
        }

        public override async UniTask Kill()
        {
            Destroy(gameObject);
            await UniTask.CompletedTask;
        }

        public override async UniTask MakeMove(Vector3 direction)
        {
            await transform.DOMove(direction, moveDelayInSec);
        }

        public override async UniTask MakeHalfMove(Vector3 direction)
        {
            var prevPosition = transform.position;
            var newDirection = Vector3.MoveTowards(prevPosition, direction, 0.5f);
            await transform.DOMove(newDirection, moveDelayInSec / 2);
            await transform.DOMove(prevPosition, moveDelayInSec / 2);
        }
    }
}