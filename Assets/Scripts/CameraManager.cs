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

        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeStrength;
        [SerializeField] private float lightChangeTimeInSec = 0.4f;
        [SerializeField] private float delayBetweenLightsInSec = 0.6f;
        [SerializeField] [Range(0.1f, 1000f)] private float delayBetweenLightsModifier = 1f;
        private bool _animating;
        private Camera _camera;

        [SerializeField] private Player _player;

        private void Start()
        {
            _camera = GetComponent<Camera>();
            if (Game.Animate)
            {
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
            await light.DOColor(new Color(1f, lifeLoss, lifeLoss), lightChangeTimeInSec / 2f);
            await light.DOColor(Color.white, lightChangeTimeInSec / 2f);
            await UniTask.Delay((int)(delayBetweenLightsInSec.ToMilliseconds() /
                                      (delayBetweenLightsModifier / Mathf.Max(0.1f, lifeLoss))));
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
            _player.OnDamage -= OnPlayerTakeDamage;
        }

        private async void OnPlayerTakeDamage(float damage)
        {
            if (!(damage >= 1)) return;
            
            await transform.DOShakeRotation(shakeDuration, shakeStrength * (damage - 0.5f));

            for (int i = 0; i < 10; i++)
            {
                _camera.orthographicSize -= 0.1f;
                await UniTask.Delay(5);
            }

            for (int i = 0; i < 10; i++)
            {
                _camera.orthographicSize += 0.1f;
                await UniTask.Delay(5);
            }
        }
    }
}