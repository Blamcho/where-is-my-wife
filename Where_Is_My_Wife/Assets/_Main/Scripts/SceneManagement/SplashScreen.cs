using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class SplashScreen : MonoBehaviour
    {
        [SerializeField] private string _mainMenuSceneName = "MainMenu";
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _stayDuration = 1f;

        private void Start()
        {
            SplashScreenAsync().Forget();
        }

        private async UniTaskVoid SplashScreenAsync()
        {
            _canvasGroup.alpha = 0;
            Sequence _splashScreen = DOTween.Sequence();
            _splashScreen.Append(_canvasGroup.DOFade(1, _fadeDuration));
            _splashScreen.AppendInterval(_stayDuration);
            _splashScreen.Append(_canvasGroup.DOFade(0, _fadeDuration));
            await _splashScreen.AsyncWaitForCompletion();
            
            LevelManager.Instance.LoadScene(_mainMenuSceneName);
        }
    }
}
