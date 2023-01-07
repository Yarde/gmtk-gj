using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Yarde.GameBoard
{
    public class ExitLevel : BoardObject
    {
        [SerializeField] private float levelLoadDelay = 1f;

        public async UniTask LoadNextLevel()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(levelLoadDelay));
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }

            SceneManager.LoadScene(nextSceneIndex);
        }
    }
}