using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Managers
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _canvas;
        [SerializeField] private Button _button;
        [SerializeField] private string _sceneName;
        private bool paused;
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
            if (paused)
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
            paused = true;
            _canvas.SetActive(true);
            Time.timeScale = 0;
        }
        
        private void ResumeGame()
        {
            paused = false;
            _canvas.SetActive(false);
            Time.timeScale = 1;
        }
        
        private void GoToMainMenu()
        {
            paused = false;
            _button.interactable = false;
            LevelManager.Instance.LoadScene(_sceneName);
        }
    }
}
