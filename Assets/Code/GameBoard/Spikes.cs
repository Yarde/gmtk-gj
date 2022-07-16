using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    internal sealed class Spikes : EnemyBase
    {
        [SerializeField] private float hp;
        [SerializeField] private float damage;

        public override float Hp => hp;
        public override float Damage => damage;
        public override Vector3 GetEnemyMove() => Vector3.zero;
        
        // spikes don't move and cannot be killed
        public override UniTask Kill() => throw new NotImplementedException();
        public override UniTask MakeMove(Vector3 direction) => throw new NotImplementedException();
        public override UniTask MakeHalfMove(Vector3 direction) => throw new NotImplementedException();
    }
}
