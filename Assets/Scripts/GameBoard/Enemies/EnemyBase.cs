using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Yarde.GameBoard
{
    public abstract class EnemyBase : BoardObject
    {
        [SerializeField] private float hp;
        [SerializeField] private float damage;
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] protected int turnsToMove = 2;

        public float Hp => hp;
        public float Damage => damage;


        protected virtual void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Count > 0)
            {
                spriteRenderer.sprite = sprites[(int)Mathf.Max(Damage - 1, 0)];
            }
        }

        public abstract Vector3 GetEnemyMove();
        public abstract UniTask Kill();
        public abstract UniTask MakeMove(Vector3 direction);
        public abstract UniTask MakeHalfMove(Vector3 direction);
    }
}