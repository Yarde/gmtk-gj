using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace Yarde.GameBoard
{
    public class Door : ObstacleBase
    {
        [SerializeField] private List<Sprite> sprites;
        [SerializeField] private Player _player;

        protected void Start()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null && sprites.Count > 0)
            {
                spriteRenderer.sprite = sprites[(int)Mathf.Max(Hp - 1, 0)];
            }
        }

        public override bool OnTouch()
        {
            if (_player.TopSide >= Hp)
            {
                Destroy(gameObject);
                return true;
            }

            return false;
        }
    }
}