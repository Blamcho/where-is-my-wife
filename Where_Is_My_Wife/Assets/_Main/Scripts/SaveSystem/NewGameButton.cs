using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Managers
{
    [RequireComponent(typeof(Button))]
    public class NewGameButton : MenuButton
    {
        [SerializeField] private string _firstLevelName; 
        
        private void Start()
        {
            _button.onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            DataSaveManager.Instance.DeleteSaveData();
            LevelManager.Instance.SetStoryMode(true);
            LevelManager.Instance.LoadScene(_firstLevelName);

            _button.interactable = false;
        }
    }
}