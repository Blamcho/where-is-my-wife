using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Managers
{
    [RequireComponent(typeof(Button))]
    public class GoToLastPlayedStoryModeLevelButton : MenuButton
    {
        private void Start()
        {
            _button.onClick.AddListener(ContinueGame);
        }

        private void ContinueGame()
        {
            _button.interactable = false;
            
            string lastPlayedStoryModeLevel = DataSaveManager.Instance.GetData<string>(DataSaveManager.LastPlayedStoryModeLevelSceneNameKey);

            if (string.IsNullOrEmpty(lastPlayedStoryModeLevel))
            {
                lastPlayedStoryModeLevel = LevelManager.FirstLevelSceneName;
            }
            
            LevelManager.Instance.SetStoryMode(true);
            LevelManager.Instance.LoadScene(lastPlayedStoryModeLevel);
        }
    }
}