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

        private void Start()
        {
            SplashScreenAsync().Forget();
        }

        private async UniTaskVoid SplashScreenAsync()
        {
            Sequence _splashScreen = DOTween.Sequence();
            _splashScreen.Append(_canvasGroup.DOFade(1, 2f));
            await _splashScreen.AsyncWaitForCompletion();

            LevelManager.Instance.LoadScene(_mainMenuSceneName);
        }
    }
}
