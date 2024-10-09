using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace WhereIsMyWife.Managers
{
    public class LevelManager : Singleton<LevelManager>
    { 
        [SerializeField] private CanvasGroup _loadingBackground;
        
        public void LoadScene(string sceneName)
        {
            LoadSceneAsync(sceneName).Forget();
        }
        
        private async UniTaskVoid LoadSceneAsync(string sceneName)
        {
            _loadingBackground.alpha = 0;
            _loadingBackground.gameObject.SetActive(true);
            await _loadingBackground.DOFade(1f, 0.5f).AsyncWaitForCompletion();
            
            var asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            await asyncOperation.ToUniTask();

            await _loadingBackground.DOFade(0f, 0.5f).AsyncWaitForCompletion();
            _loadingBackground.gameObject.SetActive(false);
        }
    }
}

