using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using VContainer;
using Yarde.GameBoard;
using Yarde.Utils.Extensions;

namespace Yarde
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private float smoothSpeed = 10f;
        [SerializeField] private Vector3 offset;
        [SerializeField] private Light light;
        private bool _animating;

        [Inject] private Player _player;

        private void Awake()
        {
            if (Game.Animate)
            {
                _player.OnKill += OnPlayerKill;
                _player.OnDamage += OnPlayerTakeDamage;
            }
        }

        private async void Update()
        {
            if (_animating || !Game.Animate)
            {
                return;
            }

            _animating = true;
            float lifeLoss = _player.HealthPoints / _player.MaxHealthPoints;
            await light.DOColor(new Color(1f, lifeLoss, lifeLoss), 0.2f);
            await light.DOColor(Color.white, 0.2f);
            await UniTask.Delay(600);
            _animating = false;
        }

        private void FixedUpdate()
        {
            Vector3 destination = _player.transform.position + offset;
            Vector3 position = transform.position;
            Vector3 smoothed = Vector3.Lerp(position, destination, smoothSpeed * Time.deltaTime);
            position = smoothed.WithY(position.y);
            transform.position = position;
        }

        private void OnDestroy()
        {
            _player.OnKill -= OnPlayerKill;
            _player.OnDamage -= OnPlayerTakeDamage;
        }

        private void OnPlayerTakeDamage(float damage)
        {
            transform.DOShakeRotation(0.3f, 0.2f * (damage - 0.5f));
        }

        private void OnPlayerKill()
        {
            transform.DOShakeRotation(1f, 0.2f);
        }
    }
}
