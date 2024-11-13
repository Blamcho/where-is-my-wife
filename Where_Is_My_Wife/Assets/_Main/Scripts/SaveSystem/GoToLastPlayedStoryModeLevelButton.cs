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
            GetComponent<Button>().onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
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