using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Yarde.GameBoard
{
    public class Door : ObstacleBase
    {
        [SerializeField] private List<Sprite> sprites;
        [Inject] private Player _player;

        protected void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Count > 0) spriteRenderer.sprite = sprites[(int)Mathf.Max(Hp - 1, 0)];
        }

        public override UniTask OnTouch()
        {
            if (_player.TopSide >= Hp)
                Destroy(gameObject);
            return UniTask.CompletedTask;
        }
    }
}