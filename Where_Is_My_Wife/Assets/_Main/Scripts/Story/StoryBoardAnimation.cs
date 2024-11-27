using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class StoryBoardAnimation : MonoBehaviour
    {
        [SerializeField] private Button _nextSceneButton;
        [SerializeField] private Image[] comicImages; 
        [SerializeField] private float fadeDuration = 1f; 
        [SerializeField] private float intervalBetweenImages = 2f;
        [SerializeField] private GameObject _skipPrompt;
        
        private int _submitPressCount = 0;
        private CancellationTokenSource _skipCTS;
        
        private void Start()
        {
            StoryBoardAnimationAsync().Forget();
            
            InputEventManager.Instance.SubmitStartAction += AdvanceSkipPrompt;
        }
        
        private void OnDestroy()
        {
            InputEventManager.Instance.SubmitStartAction -= AdvanceSkipPrompt;
        }

        private async UniTaskVoid StoryBoardAnimationAsync()
        {
            _nextSceneButton.interactable = false;
            _nextSceneButton.gameObject.SetActive(false);
            foreach (var image in comicImages)
            {
                image.color = new Color(1, 1, 1, 0);
            }

            _skipCTS?.Cancel();
            _skipCTS = new CancellationTokenSource();
            
            for (int i = 0; i < comicImages.Length; i++)
            {
                comicImages[i].DOFade(1, fadeDuration); 
                await UniTask.Delay(TimeSpan.FromSeconds(fadeDuration + intervalBetweenImages), cancellationToken: _skipCTS.Token);
            }
            
            ShowContinueButton();
        }

        private void ShowContinueButton()
        {
            _skipPrompt.SetActive(false);
            _nextSceneButton.gameObject.SetActive(true);
            _nextSceneButton.interactable = true;
            _nextSceneButton.Select();
        }

        private void AdvanceSkipPrompt()
        {
            _submitPressCount++;
            
            if (_submitPressCount == 1)
            {
                _skipPrompt.SetActive(true);
                return;
            }
            
            SkipAsync().Forget();
        }
        
        private async UniTaskVoid SkipAsync()
        {
            InputEventManager.Instance.SubmitStartAction -= AdvanceSkipPrompt;
            
            _skipPrompt.SetActive(false);
            
            foreach (var image in comicImages)
            {
                image.color = Color.white;
            }

            await UniTask.DelayFrame(1); // Prevent instant button press
            
            _skipCTS?.Cancel();
            ShowContinueButton();
        }
    }
}
