using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Yarde.GameBoard.Enemies
{
    public class EnemyFollowingPath : EnemyBase
    {
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float moveDelayInSec = 1;
        [SerializeField] private float movementSpeed = 1;
        [SerializeField] private bool loop = true;
        private int _queueDirection = 1;
        private Vector3 _targetPosition;
        private int _waypointIndex;

        private void Start()
        {
            SetInitValues();
            CheckWaypoint();
        }

        private void SetInitValues()
        {
            transform.position = waypoints[_waypointIndex].position;
            _waypointIndex++;
            _targetPosition = waypoints[_waypointIndex].position;
        }

        private void CheckWaypoint()
        {
            if (loop)
                LoopWaypointsList();
            else
                FlipWaypointsList();
            
            if (transform.position == _targetPosition)
            {
                _waypointIndex += _queueDirection;
                _targetPosition = waypoints[_waypointIndex].position;
            }
        }

        private void LoopWaypointsList()
        {
            if (_waypointIndex == waypoints.Count - 1) _waypointIndex = -1;
        }

        private void FlipWaypointsList()
        {
            if (_waypointIndex == waypoints.Count - 1)
                _queueDirection = -1;
            else if (_waypointIndex == 0)
                _queueDirection = 1;
        }

        public override Vector3 GetEnemyMove()
        {
            return Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed);
        }

        public override UniTask Kill()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }

        public override async UniTask MakeMove(Vector3 direction)
        {
            await transform.DOMove(direction, moveDelayInSec);
            CheckWaypoint();
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