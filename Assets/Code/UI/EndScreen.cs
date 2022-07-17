using UnityEngine;
using UnityEngine.SceneManagement;
using Yarde.Utils.Logger;

namespace Yarde
{
    public class EndScreen : MonoBehaviour
    {
        [SerializeField] private int timeToRestartGameSec = 10;

        private void Start()
        {
            this.LogVerbose($"RestartGame in {timeToRestartGameSec}s");
            Invoke(nameof(RestartGame), timeToRestartGameSec);
        }

        private void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}