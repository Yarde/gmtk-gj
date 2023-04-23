using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Yarde.Utils.Extensions;
using Yarde.Utils.Logger;
using Logger = Yarde.Utils.Logger.Logger;

namespace Yarde.GameBoard
{
    [LogSettings(color: "#8CC")]
    public class Game : MonoBehaviour
    {
        public static bool Paused;
        public static bool Animate;

        [SerializeField] private LoggerLevel loggerLevel = LoggerLevel.Error;

        [Header("Game settings")] [SerializeField]
        private int millisBetweenSyncs = 1000;

        [SerializeField] private float timeDamage = 1;
        [SerializeField] private bool autoEnemyMove;
        [SerializeField] private bool timeLoseLive = true;
        [SerializeField] private bool animateCamera;
        [SerializeField] private List<AudioSource> audioSources;
        [SerializeField] private AudioClip diceRollAudio;
        [SerializeField] private AudioClip diceBlockedAudio;
        [SerializeField] private AudioClip diceHitAudio;
        [SerializeField] private AudioClip levelWinAudio;

        [Header("Particles")] [SerializeField] private ParticleSystem enemyKillParticle;
        private CollectibleBase[] _collectibles;
        private EnemyBase[] _enemies;
        private ExitLevel _exitLevel;
        private int _currentSource;

        [SerializeField] private InputManager _inputManager;

        private ObstacleBase[] _obstacles;
        [SerializeField] private Player _player;
        private bool _waiting;

        private void Awake()
        {
            Logger.Level = loggerLevel;
            Application.targetFrameRate = 60;

            _obstacles = GetComponentsInChildren<ObstacleBase>();
            _enemies = GetComponentsInChildren<EnemyBase>();
            _collectibles = GetComponentsInChildren<CollectibleBase>();
            _exitLevel = FindObjectOfType<ExitLevel>();

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

        private async UniTask MakeTurn(Vector3 direction)
        {
            if (await CheckWin(direction))
            {
                return;
            }

            await MakePlayerTurn(direction);

            if (!autoEnemyMove)
            {
                await MakeEnemyTurn();
            }

            _player.AddPoints(1);
        }

        private async UniTask<bool> CheckWin(Vector3 direction)
        {
            if (IsExitLevel(direction))
            {
                await UniTask.WhenAll(_exitLevel.LoadNextLevel(), _player.Roll(direction));
                PlaySfx(levelWinAudio);
                return true;
            }

            return false;
        }

        private async UniTask MakePlayerTurn(Vector3 direction)
        {
            this.LogVerbose($"Top side of dice is {_player.TopSide}");

            if (TryGetObstacle(_player.Transform.position + direction, _player.Size, out var touchedObstacle))
            {
                await MakeHalfMove(direction, touchedObstacle);
            }
            else
            {
                await MakeMove(direction);
            }
        }

        private async UniTask MakeMove(Vector3 direction)
        {
            EnemyBase attackedEnemy = CheckAttackedEnemy(direction);
            if (attackedEnemy)
            {
                await AttackEnemy(direction, attackedEnemy);
                PlaySfx(attackedEnemy.soundOnPlayerHit);
            }
            else
            {
                await _player.Roll(direction);
                PlaySfx(diceRollAudio);
            }

            TryCollectItems();
        }

        private async UniTask MakeHalfMove(Vector3 direction, ObstacleBase touchedObstacle)
        {
            if (touchedObstacle.OnTouch())
            {
                _obstacles = _obstacles.Where(e => e != touchedObstacle).ToArray();
                await _player.Roll(direction);
                PlaySfx(diceRollAudio);
            }
            else
            {
                await _player.HalfRoll(direction);
                PlaySfx(diceBlockedAudio);
            }

            PlaySfx(touchedObstacle.soundOnPlayerHit);
        }

        private void PlaySfx(AudioClip clip)
        {
            if (!clip || audioSources.Any(a => !a))
            {
                return;
            }

            audioSources[_currentSource].clip = clip;
            audioSources[_currentSource].Play();

            _currentSource++;
            if (_currentSource >= audioSources.Count)
            {
                _currentSource = 0;
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
                PlaySfx(diceHitAudio);
                await UniTask.WhenAll(
                    _player.Roll(direction),
                    attackedEnemy.Kill()
                );
            }
            else
            {
                PlaySfx(diceBlockedAudio);
                PlaySfx(attackedEnemy.soundOnPlayerHit);
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
                this.LogVerbose(
                    $"Enemy: {enemy.gameObject.FullPath()} has move? {destination.magnitude > 0f}, {destination}");
                if (destination.sqrMagnitude > 0f)
                {
                    // todo probably it is not even used, but it is really expensive
                    // if (TryGetObstacle(destination, enemy.Size, out _))
                    // {
                    //     this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} blocked, do HalfMove");
                    //     enemyMoves.Add(enemy.MakeHalfMove(destination));
                    // }
                    // else 
                    if (enemy.CheckCollision(destination, enemy.Size, _player.Transform.position, _player.Size))
                    {
                        this.LogVerbose($"Enemy: {enemy.gameObject.FullPath()} hit player, do HalfMove");
                        enemyMoves.Add(_player.TakeDamage(enemy.Damage));
                        enemyMoves.Add(enemy.MakeHalfMove(destination));
                        PlaySfx(enemy.soundOnPlayerHit);
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
            Vector3 destination = _player.Transform.position + vector3;
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
            Vector3 destination = _player.Transform.position + vector3;
            if (!_exitLevel || !_exitLevel.CheckCollision(destination, _player.Size))
            {
                return false;
            }

            this.Log("Exit Level");
            return true;
        }

        private bool TryGetObstacle(Vector3 destination, Vector2 size, out ObstacleBase obstacle)
        {
            obstacle = null;
            foreach (ObstacleBase o in _obstacles)
            {
                if (o.CheckCollision(destination, size))
                {
                    this.Log($"Path Blocked by Obstacle: {o.name}");
                    obstacle = o;
                    return true;
                }
            }

            return false;
        }

        private void TryCollectItems()
        {
            foreach (CollectibleBase collectible in _collectibles)
            {
                if (collectible.CheckCollision(_player.Transform.position, _player.Size))
                {
                    this.Log($"Collectible found: {collectible.name}");
                    _player.CollectItem(collectible.Collect());
                }
            }
        }
    }
}