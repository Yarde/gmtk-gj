using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Yarde.Utils.Logger;

namespace Yarde.GameBoard
{
    public class Game : MonoBehaviour
    {
        [Inject] private InputManager _inputManager;
        [Inject] private Player _player;

        private ObstacleBase[] _obstacles;
        private EnemyBase[] _enemies;
        private CollectibleBase[] _collectibles;

        private void Awake()
        {
            _obstacles = GetComponentsInChildren<ObstacleBase>();
            _enemies = GetComponentsInChildren<EnemyBase>();
            _collectibles = GetComponentsInChildren<CollectibleBase>();
        }

        private void Start()
        {
            _inputManager.OnNewTurn += MakeTurn;
        }

        private async UniTask MakeTurn(Vector3 arg)
        {
            await MakePlayerTurn(arg);
            await MakeEnemyTurn();
        }

        private async Task MakePlayerTurn(Vector3 arg)
        {
            bool free = CheckIfPathIsFree(arg);
            if (free)
            {
                EnemyBase killedEnemy = CheckKilledEnemy(arg);
                UniTask roll = _player.Roll(arg);
                if (killedEnemy != null)
                {
                    await _player.OnEnemyKilled(killedEnemy);
                    await killedEnemy.Kill();
                }
                await roll;
            }
            else
            {
                await _player.HalfRoll(arg);
            }

            await TryCollectItems();
        }

        private async UniTask MakeEnemyTurn()
        {
            var enemyMoves = new List<UniTask>();
            foreach (EnemyBase enemy in _enemies)
            {
                Vector3 direction = enemy.GetEnemyMove();
                if (direction.magnitude > 0f)
                {
                    bool hit = enemy.CheckPlayerHit(direction);
                    if (hit)
                    {
                        _player.TakeDamage(enemy.Damage);
                        enemyMoves.Add(enemy.MakeHalfMove(direction));
                    }
                    else
                    {
                        enemyMoves.Add(enemy.MakeMove(direction));
                    }
                }
            }

            await UniTask.WhenAll(enemyMoves);
        }

        private EnemyBase CheckKilledEnemy(Vector3 vector3)
        {
            Vector3 destination = _player.transform.position + vector3;
            foreach (EnemyBase enemy in _enemies)
            {
                if (enemy.CheckCollision(destination, _player.Size))
                {
                    this.Log($"Enemy: {enemy.name} killed!");
                    return enemy;
                }
            }
            return null;
        }

        private bool CheckIfPathIsFree(Vector3 vector3)
        {
            Vector3 destination = _player.transform.position + vector3;
            foreach (ObstacleBase obstacle in _obstacles)
            {
                if (obstacle.CheckCollision(destination, _player.Size))
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
