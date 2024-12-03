using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Managers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Button _resumeButton;
        [SerializeField] private Button _mainmenuButton;
        
        private MenuButton _resumeMenuButton;
        
        private bool _isPaused = false;
        private bool _isGoingToMainMenu = false;
        
        private void Start()
        {
            InputEventManager.Instance.PauseStartAction += DeterminedPaused;
            _resumeMenuButton = _resumeButton.GetComponent<MenuButton>();
            _mainmenuButton.onClick.AddListener(GoToMainMenu);
            _resumeButton.onClick.AddListener(ResumeGame);
        }

        private void OnDestroy()
        {
            InputEventManager.Instance.PauseStartAction -= DeterminedPaused;
        }

        private void DeterminedPaused()
        {
            if (_isGoingToMainMenu) return;
            
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
            _resumeMenuButton.SetSelectedColor();
            GameManager.Instance.Pause();
            AudioManager.Instance.PlaySFX("Pause");
        }
        
        private void ResumeGame()
        {
            if (_isGoingToMainMenu) return;
            
            _isPaused = false;
            _canvas.SetActive(false);
            GameManager.Instance.Resume();
            AudioManager.Instance.PlaySFX("Resume");
        }
        
        private void GoToMainMenu()
        {
            _isGoingToMainMenu = true;
            
            _mainmenuButton.interactable = false;
            GameManager.Instance.GoToMainMenu();
        }
    }
}
