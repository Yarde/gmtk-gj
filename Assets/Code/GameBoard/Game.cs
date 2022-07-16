using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Yarde.Utils.Logger;

namespace Yarde.GameBoard
{
    [LogSettings(color: "#8CC")]
    public class Game : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;
        [Inject] private Player _player;

        private ObstacleBase[] _obstacles;
        private EnemyBase[] _enemies;
        private CollectibleBase[] _collectibles;
        public static bool Paused { get; set; }

        private void Awake()
        {
            _obstacles = GetComponentsInChildren<ObstacleBase>();
            _enemies = GetComponentsInChildren<EnemyBase>();
            _collectibles = GetComponentsInChildren<CollectibleBase>();
        }

        private void Start()
        {
            Paused = false;
            _inputManager.OnNewTurn += MakeTurn;
        }

        private async UniTask MakeTurn(Vector3 arg)
        {
            await MakePlayerTurn(arg);
            await MakeEnemyTurn();
            _player.TakeDamage(1);
            _player.AddPoints(1);
        }

        private async UniTask MakePlayerTurn(Vector3 direction)
        {
            this.LogVerbose($"Top side of dice is {_player.TopSide}");
            if (CheckIfPathIsFree(_player.transform.position + direction, _player.Size))
            {
                EnemyBase attackedEnemy = CheckAttackedEnemy(direction);
                if (attackedEnemy != null)
                {
                    await AttackEnemy(direction, attackedEnemy);
                }
                else
                {
                    await _player.Roll(direction);
                }
                await TryCollectItems();
            }
            else
            {
                // obstacle hit
                await _player.HalfRoll(direction);
            }
        }

        private async UniTask AttackEnemy(Vector3 direction, EnemyBase attackedEnemy)
        {
            // check if enemy will be killed or not
            if (attackedEnemy.Hp <= _player.TopSide)
            {
                await UniTask.WhenAll(
                    _player.Roll(direction),
                    _player.OnEnemyKilled(attackedEnemy.Damage),
                    attackedEnemy.Kill()
                );
            }
            else
            {
                await _player.HalfRoll(direction);
                _player.TakeDamage(attackedEnemy.Damage);
            }
        }

        private async UniTask MakeEnemyTurn()
        {
            var enemyMoves = new List<UniTask>();
            foreach (EnemyBase enemy in _enemies)
            {
                Vector3 destination = enemy.GetEnemyMove();
                if (destination.magnitude > 0f )
                {
                    if (!CheckIfPathIsFree(destination, enemy.Size))
                    {
                        enemyMoves.Add(enemy.MakeHalfMove(destination));
                    }
                    else if (enemy.CheckCollision(destination, enemy.Size, _player.transform.position, _player.Size))
                    {
                        _player.TakeDamage(enemy.Damage);
                        enemyMoves.Add(enemy.MakeHalfMove(destination));
                    }
                    else
                    {
                        enemyMoves.Add(enemy.MakeMove(destination));
                    }
                }
            }

            await UniTask.WhenAll(enemyMoves);
        }

        private EnemyBase CheckAttackedEnemy(Vector3 vector3)
        {
            Vector3 destination = _player.transform.position + vector3;
            foreach (EnemyBase enemy in _enemies)
            {
                if (enemy.CheckCollision(destination, _player.Size))
                {
                    this.Log($"Enemy: {enemy.name} attacked!");
                    return enemy;
                }
            }
            return null;
        }

        private bool CheckIfPathIsFree(Vector3 destination, Vector2 size)
        {
            foreach (ObstacleBase obstacle in _obstacles)
            {
                if (obstacle.CheckCollision(destination, size))
                {
                    this.Log($"Path Blocked by Obstacle: {obstacle.name}");
                    return false;
                }
            }
            return true;
        }

        private async UniTask TryCollectItems()
        {
            foreach (CollectibleBase collectible in _collectibles)
            {
                if (collectible.CheckCollision(_player.transform.position, _player.Size))
                {
                    this.Log($"Collectible found: {collectible.name}");
                    await _player.CollectItem(collectible.Collect());
                }
            }
        }
    }
}
