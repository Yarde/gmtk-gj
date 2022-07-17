using UnityEngine;
using UnityEngine.SceneManagement;
using Yarde.Utils.Logger;

namespace Yarde
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private float levelLoadDelay = 1f;

        public void StartGame()
        {
            this.LogVerbose($"RestartGame in {levelLoadDelay}s");
            Invoke(nameof(LoadNextLvl), levelLoadDelay);
        }

        private void LoadNextLvl()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            var nextSceneIndex = currentSceneIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}