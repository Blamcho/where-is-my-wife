using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Managers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainmenuButton;
        
        private bool _isPaused;
        
        private void Start()
        {
            InputEventManager.Instance.PauseStartAction += DeterminedPaused;
            _mainmenuButton.onClick.AddListener(GoToMainMenu);
            _resumeButton.onClick.AddListener(ResumeGame);
        }

        private void OnDestroy()
        {
            InputEventManager.Instance.PauseStartAction -= DeterminedPaused;
        }

        private void DeterminedPaused()
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            _isPaused = true;
            _canvas.SetActive(true);
            _resumeButton.Select();
            GameManager.Instance.Pause();
        }
        
        private void ResumeGame()
        {
            _isPaused = false;
            _canvas.SetActive(false);
            GameManager.Instance.Resume();
        }
        
        private void GoToMainMenu()
        {
            _isPaused = false;
            _mainmenuButton.interactable = false;
            LevelManager.Instance.LoadScene(LevelManager.MainMenuSceneName);
        }
    }
}
