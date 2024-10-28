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
        [SerializeField] private float _completelyVisibleTime = 2f;
        [SerializeField] private float _fadeTime = 1f;

        private void Start()
        {
            SplashScreenAsync().Forget();
        }

        private async UniTaskVoid SplashScreenAsync()
        {
            _canvasGroup.alpha = 0;
            Sequence _splashScreen = DOTween.Sequence();
            _splashScreen.Append(_canvasGroup.DOFade(1, _fadeTime));
            _splashScreen.AppendInterval(_completelyVisibleTime);
            _splashScreen.Append(_canvasGroup.DOFade(0, _fadeTime));
            await _splashScreen.AsyncWaitForCompletion();

            LevelManager.Instance.LoadScene(_mainMenuSceneName);
        }
    }
}
