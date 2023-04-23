using UnityEngine;
using UnityEngine.UI;

namespace Yarde.UI
{
    public class Clock : MonoBehaviour
    {
        [SerializeField] private Image _clock;
        [SerializeField] private Player _player;

        private void Update()
        {
            _clock.fillAmount = _player.HealthPoints / _player.MaxHealthPoints;
        }
    }
}