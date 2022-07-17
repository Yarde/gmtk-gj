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
        private int _turn;
        private int _waypointIndex;

        protected override void Start()
        {
            base.Start();
            SetInitValues();
            CheckWaypoint();
        }

        private void SetInitValues()
        {
            Vector3 firstWaypoint = waypoints[_waypointIndex].position;
            firstWaypoint.y = transform.position.y;
            transform.position = firstWaypoint;
            _waypointIndex++;
            _targetPosition = waypoints[_waypointIndex].position;
        }

        private void CheckWaypoint()
        {
            if (loop)
            {
                LoopWaypointsList();
            }
            else
            {
                FlipWaypointsList();
            }

            if (transform && transform.position == _targetPosition)
            {
                _waypointIndex += _queueDirection;
                _targetPosition = waypoints[_waypointIndex].position;
            }
        }

        private void LoopWaypointsList()
        {
            if (_waypointIndex == waypoints.Count - 1)
            {
                _waypointIndex = -1;
            }
        }

        private void FlipWaypointsList()
        {
            if (_waypointIndex == waypoints.Count - 1)
            {
                _queueDirection = -1;
            }
            else if (_waypointIndex == 0)
            {
                _queueDirection = 1;
            }
        }

        public override Vector3 GetEnemyMove()
        {
            _turn++;
            if (_turn % turnsToMove == 0)
            {
                _targetPosition.y = transform.position.y;
                return Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed);
            }

            return Vector3.zero;
        }

        public override UniTask Kill()
        {
            Destroy(gameObject);
            return UniTask.CompletedTask;
        }

        public override async UniTask MakeMove(Vector3 direction)
        {
            if (!transform)
            {
                return;
            }
            await transform.DOMove(direction, moveDelayInSec);
            CheckWaypoint();
        }

        public override async UniTask MakeHalfMove(Vector3 direction)
        {
            if (!transform)
            {
                return;
            }
            Vector3 prevPosition = transform.position;
            Vector3 newDirection = Vector3.MoveTowards(prevPosition, direction, 0.5f);
            await transform.DOMove(newDirection, moveDelayInSec / 2);
            await transform.DOMove(prevPosition, moveDelayInSec / 2);
        }
    }
}
