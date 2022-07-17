using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;
using Yarde.Utils.Extensions;
using Yarde.Utils.Logger;

namespace Yarde.GameBoard
{
    [LogSettings(color: "#8CC")]
    public class Game : MonoBehaviour
    {
        public static bool Paused;
        public static bool Animate;
        [Header("Game settings")]
        [SerializeField] private int millisBetweenSyncs = 1000;
        [SerializeField] private float timeDamage = 1;
        [SerializeField] private bool autoEnemyMove;
        [SerializeField] private bool timeLoseLive = true;
        [SerializeField] private bool moveLoseLive;
        [SerializeField] private bool animateCamera;
        
        [Header("Particles")]
        private ParticleSystem enemyKillParticle;
        private CollectibleBase[] _collectibles;
        private EnemyBase[] _enemies;
        private ExitLevel _exitLevel;

        [Inject] private InputManager _inputManager;

        private ObstacleBase[] _obstacles;
        [Inject] private Player _player;
        private bool _waiting;

        private void Awake()
        {
            _obstacles = GetComponentsInChildren<ObstacleBase>();
            _enemies = GetComponentsInChildren<EnemyBase>();
            _collectibles = GetComponentsInChildren<CollectibleBase>();
            _exitLevel = FindObjectOfType<ExitLevel>();

            enemyKillParticle = FindObjectOfType<ParticleSystem>();
            
            Paused = false;
            Animate = animateCamera;
        }

        private void Start()
        {
            _inputManager.OnNewTurn += MakeTurn;
        }

        private async void Update()
        {
            if (_waiting)
            {
                return;
            }
            await GameTick();
        }

        private async UniTask GameTick()
        {
            _waiting = true;
            if (timeLoseLive)
            {
                await _player.TakeDamage(timeDamage);
            }

            if (autoEnemyMove)
            {
                await UniTask.WhenAll(
                    MakeEnemyTurn(),
                    UniTask.Delay(millisBetweenSyncs));
            }
            else
            {
                await UniTask.Delay(millisBetweenSyncs);
            }
            _waiting = false;
        }

        private async UniTask MakeTurn(Vector3 arg)
        {
            await MakePlayerTurn(arg);
            if (!autoEnemyMove)
            {
                await MakeEnemyTurn();
            }
            _player.AddPoints(1);
            if (moveLoseLive)
            {
                await _player.TakeDamage(timeDamage);
            }
        }

        private async UniTask MakePlayerTurn(Vector3 direction)
        {
            this.LogVerbose($"Top side of dice is {_player.TopSide}");
            if (IsExitLevel(direction))
            {
                await UniTask.WhenAll(_exitLevel.LoadNextLevel(), _player.Roll(direction));
                return;
            }
            ObstacleBase touchedObstacle = CheckIfPathIsFree(_player.transform.position + direction, _player.Size);
            if (touchedObstacle == null)
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
                if (touchedObstacle.OnTouch())
                {
                    _obstacles = _obstacles.Where(e => e != touchedObstacle).ToArray();
                    await _player.Roll(direction);
                }
                else
                {
                    await _player.HalfRoll(direction);
                }
            }
        }

        private async UniTask AttackEnemy(Vector3 direction, EnemyBase attackedEnemy)
        {
            // check if enemy will be killed or not
            if (attackedEnemy.Hp <= _player.TopSide)
            {
                _enemies = _enemies.Where(e => e != attackedEnemy).ToArray();
                PlayParticle(attackedEnemy);
                _player.OnEnemyKilled(attackedEnemy.Damage);
                await UniTask.WhenAll(
                    _player.Roll(direction),
                    attackedEnemy.Kill()
                );
            }
            else
            {
                await UniTask.WhenAll(
                    _player.HalfRoll(direction),
                    _player.TakeDamage(attackedEnemy.Damage)
                );
            }
        }

        private void PlayParticle(EnemyBase attackedEnemy)
        {
            if (enemyKillParticle)
            {
                enemyKillParticle.transform.position = attackedEnemy.transform.position;
                enemyKillParticle.Play();
            }
        }

        private async UniTask MakeEnemyTurn()
        {
            var enemyMoves = new List<UniTask>();
            foreach (EnemyBase enemy in _enemies)
            {
                Vector3 destination = enemy.GetEnemyMove();
                this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} has move? {destination.magnitude > 0f}, {destination}");
                if (destination.magnitude > 0f)
                {
                    if (CheckIfPathIsFree(destination, enemy.Size) != null)
                    {
                        this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} blocked, do HalfMove");
                        enemyMoves.Add(enemy.MakeHalfMove(destination));
                    }
                    else if (enemy.CheckCollision(destination, enemy.Size, _player.transform.position, _player.Size))
                    {
                        this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} hit player, do HalfMove");
                        enemyMoves.Add(_player.TakeDamage(enemy.Damage));
                        enemyMoves.Add(enemy.MakeHalfMove(destination));
                    }
                    else
                    {
                        this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} do normal Move");
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

        private bool IsExitLevel(Vector3 vector3)
        {
            Vector3 destination = _player.transform.position + vector3;
            if (!_exitLevel || !_exitLevel.CheckCollision(destination, _player.Size))
            {
                return false;
            }
            this.Log("Exit Level");
            return true;
        }

        private ObstacleBase CheckIfPathIsFree(Vector3 destination, Vector2 size)
        {
            foreach (ObstacleBase obstacle in _obstacles)
            {
                if (obstacle.CheckCollision(destination, size))
                {
                    this.Log($"Path Blocked by Obstacle: {obstacle.name}");
                    return obstacle;
                }
            }

            return null;
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
