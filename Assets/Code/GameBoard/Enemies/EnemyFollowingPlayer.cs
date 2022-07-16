using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Yarde.GameBoard;

namespace Yarde
{
    public class EnemyFollowingPlayer  : EnemyBase
    {
        [Inject] private Player _player;
        [SerializeField] private float moveDelayInSec = 1;
        [SerializeField] private float movementSpeed = 1;
        private Vector3 _targetPosition;
        private int waypointIndex;

        private Vector3 FollowPlayer()
        {
            _targetPosition = _player.transform.position;
            var result = Vector3.MoveTowards(transform.position, _targetPosition, movementSpeed);
            return result;
        }

        public override Vector3 GetEnemyMove()
        {
            return FollowPlayer();
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
        }

        public override async UniTask MakeHalfMove(Vector3 direction)
        {
            var prevPosition = transform.position;
            var newDirection = Vector3.MoveTowards(prevPosition, direction, 0.5f);
            await transform.DOMove(newDirection, moveDelayInSec/2);
            await transform.DOMove(prevPosition, moveDelayInSec/2);
        }
    }
}
