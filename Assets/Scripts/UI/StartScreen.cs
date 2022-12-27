using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Yarde.Utils.Logger;

namespace Yarde
{
    public class StartScreen : MonoBehaviour
    {
        [SerializeField] private float levelLoadDelay = 1f;
        [SerializeField] private float animationDelay = 0.1f;
        [SerializeField] private float animationDuration = 0.1f;
        [SerializeField] private Color shadowColor = new Color(96,96,96,255);
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image titleImage;
        [SerializeField] private List<Image> screenImages;

        private void Awake()
        {
            HideImages();
        }
        private async UniTask Start()
        {
            await ShowTitle();
            await ShowUI();
        }

        private async UniTask ShowTitle()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(animationDelay));
            backgroundImage.DOColor(shadowColor, animationDuration);
            titleImage.DOFade(1f, animationDuration);
            await UniTask.Delay(TimeSpan.FromSeconds(animationDelay));
        } 
        private void HideImages()
        {
            titleImage.DOFade(0f, 0f);
            screenImages.ForEach(i => i.DOFade(0f, 0f));
        }

        private async UniTask ShowUI()
        {
            
            screenImages.ForEach(i => i.DOFade(1f, animationDuration));
            await UniTask.Delay(TimeSpan.FromSeconds(animationDelay));
        }

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