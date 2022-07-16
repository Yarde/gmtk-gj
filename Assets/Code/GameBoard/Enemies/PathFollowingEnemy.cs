using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Yarde.GameBoard.Enemies
{
    public class PathFollowingEnemy : EnemyBase
    {
        [SerializeField] private List<Transform> waypoints;
        [SerializeField] private float moveDelayInSec = 1;
        [SerializeField] private float movementSpeed = 1;
        private float _maxDistanceDelta;
        private Vector3 _targetPosition;
        private int queueDirection = 1;
        private int waypointIndex;

        private void Start()
        {
            SetInitValues();
            CheckWaypoint();
        }

        private void SetInitValues()
        {
            transform.position = waypoints[waypointIndex].position;
            waypointIndex++;
            _targetPosition = waypoints[waypointIndex].position;
        }

        private void CheckWaypoint()
        {
            if (waypointIndex == waypoints.Count - 1)
                queueDirection = -1;
            if (waypointIndex == 0)
                queueDirection = 1;
            if (transform.position == _targetPosition)
            {
                waypointIndex += queueDirection;
                _targetPosition = waypoints[waypointIndex].position;
            }
        }

        private Vector3 FollowPath()
        {
            var result = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed);
            return result;
        }

        public override Vector3 GetEnemyMove()
        {
            return FollowPath();
        }

        public override bool CheckPlayerHit(Vector3 direction)
        {
            return false;
        }

        public override UniTask Kill()
        {
            throw new NotImplementedException();
        }

        public override async UniTask MakeMove(Vector3 direction)
        {
            await transform.DOMove(direction, moveDelayInSec);
            CheckWaypoint();
        }

        public override async UniTask MakeHalfMove(Vector3 direction)
        {
            var startPosition = transform.position;
            var halfDir = Vector3.MoveTowards(startPosition, direction, 0.5f);
            await transform.DOMove(halfDir, moveDelayInSec/2);
            await transform.DOMove(startPosition, moveDelayInSec/2);
            CheckWaypoint();
        }
    }
}