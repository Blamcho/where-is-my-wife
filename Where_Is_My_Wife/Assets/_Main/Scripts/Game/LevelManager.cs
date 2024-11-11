using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.Managers
{
    public class LevelManager : Singleton<LevelManager>
    { 
        public const string MainMenuSceneName = "MainMenu";
        public const string FirstLevelSceneName = "Story0";
        
        [SerializeField] private CanvasGroup _loadingBackground;
        
        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName).Forget();
        }
        
        private async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            _loadingBackground.alpha = 0;
            _loadingBackground.gameObject.SetActive(true);
            await _loadingBackground.DOFade(1f, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            await asyncOperation.ToUniTask();

            await _loadingBackground.DOFade(0f, 0.5f).SetUpdate(true).AsyncWaitForCompletion();
            _loadingBackground.gameObject.SetActive(false);

            Time.timeScale = 1;
        }
    }
}

