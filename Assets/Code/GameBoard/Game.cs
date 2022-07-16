using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
                EnemyBase kill = CheckKilledEnemy(arg);
                if (kill != null)
                {
                    Destroy(kill.gameObject);
                    _player.OnEnemyKilled(kill);
                }
                await _player.Roll(arg);
            }
            else
            {
                await _player.HalfRoll(arg);
            }

            await TryCollectItems();
        }

        private async UniTask MakeEnemyTurn()
        {
            foreach (EnemyBase enemy in _enemies)
            {
                Vector3 direction = enemy.GetEnemyMove();
                if (direction.magnitude > 0f)
                {
                    bool hit = enemy.CheckPlayerHit(direction);
                    if (hit)
                    {
                        _player.TakeDamage(enemy.Damage);
                    }
                    else
                    {
                        enemy.MakeMove(direction);
                    }
                }
            }
        }

        private EnemyBase CheckKilledEnemy(Vector3 vector3)
        {
            Vector3 destination = _player.transform.position + vector3;
            foreach (EnemyBase enemy in _enemies)
            {
                float distance = Vector3.Distance(destination, enemy.transform.position);
                if (distance < 0.5f)
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
                var distance = Vector3.Distance(destination, obstacle.transform.position);
                if (distance < 0.5f)
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
                var distance = Vector3.Distance(_player.transform.position, collectible.transform.position);
                if (distance < 0.5f)
                {
                    this.Log($"Collectible found: {collectible.name}");
                    _player.CollectItem(collectible.Collect());
                }
            }
        }
    }
}
