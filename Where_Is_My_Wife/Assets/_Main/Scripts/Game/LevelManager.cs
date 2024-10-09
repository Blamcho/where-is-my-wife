using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.Managers
{
    public class LevelManager : Singleton<LevelManager>
    { 
        [SerializeField] private CanvasGroup _loadingBackground;
        [SerializeField] private string _lastSceneLoaded;
        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName).Forget();
        }
        
        private async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            _loadingBackground.alpha = 0;
            _loadingBackground.gameObject.SetActive(true);
            await _loadingBackground.DOFade(1f, 0.5f).AsyncWaitForCompletion();
            
            if (!string.IsNullOrEmpty(_lastSceneLoaded))
            {
                var unloadOperation = SceneManager.UnloadSceneAsync(_lastSceneLoaded);
                if (unloadOperation != null)
                {
                    await unloadOperation.ToUniTask();
                }
            }
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            await asyncOperation.ToUniTask();
            _lastSceneLoaded = sceneName;
            
            await _loadingBackground.DOFade(0f, 0.5f).AsyncWaitForCompletion();
            _loadingBackground.gameObject.SetActive(false);
        }
        
        
    }
}

