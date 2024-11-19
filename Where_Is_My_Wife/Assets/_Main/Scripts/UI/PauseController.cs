using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Managers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Button _button;
        
        private bool _isPaused;
        
        private void Start()
        {
            InputEventManager.Instance.PauseStartAction += DeterminedPaused;
            _button.onClick.AddListener(GoToMainMenu);
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
            _button.Select();
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
            _button.interactable = false;
            LevelManager.Instance.LoadScene(LevelManager.MainMenuSceneName);
        }
    }
}
