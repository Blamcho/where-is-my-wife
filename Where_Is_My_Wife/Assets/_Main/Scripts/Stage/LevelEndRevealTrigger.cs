using Cinemachine;
using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.SceneManagement
{
    public class LevelEndRevealTrigger : MonoBehaviour
    {
        [SerializeField] private string _nextLevelInitialScene;
        [SerializeField] private int _currentLevelNumber;
        [SerializeField] private CinemachineVirtualCamera _revealCamera;
        [SerializeField] private float _revealDuration = 3f;
    
        private bool _hasBeenTriggered = false;
        private Sequence _sequence;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && !_hasBeenTriggered)
            {
                _hasBeenTriggered = true;
            
                _sequence = DOTween.Sequence();

                _sequence.AppendCallback(() => { CameraManager.Instance.ChangeCamera(_revealCamera); });
                _sequence.AppendInterval(_revealDuration);
                _sequence.AppendCallback(() => { GoToNextLevel(); });
            }
        }

        private void GoToNextLevel()
        {
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