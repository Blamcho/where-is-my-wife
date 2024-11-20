using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class GoToNextLevelTrigger : MonoBehaviour
    {
        [SerializeField] private string _nextLevelInitialScene;
        [SerializeField] private int _currentLevelNumber;
        
        private bool _hasBeenTriggered = false;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_hasBeenTriggered)
            {   
                _hasBeenTriggered = true;

                if (LevelManager.Instance.IsInStoryMode)
                {
                    DataSaveManager.Instance.SetNextLevelParameters(_currentLevelNumber, _nextLevelInitialScene, true);
                    LevelManager.Instance.LoadScene(_nextLevelInitialScene);
                }
                else
                {
                    LevelManager.Instance.LoadScene(LevelManager.MainMenuSceneName);
                }
            }
        }
    }
}
