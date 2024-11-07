using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Managers
{
    [RequireComponent(typeof(Button))]
    public class GoToLastUnlockedLevelButton : MenuButton
    {
        [SerializeField] private string _firstLevelName = "Story0";
        
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(StartNewGame);
        }

        private void StartNewGame()
        {
            string lastUnlockedLevel = DataSaveManager.Instance.GetData<string>(DataSaveManager.LastUnlockedLevelSceneNameKey);

            if (string.IsNullOrEmpty(lastUnlockedLevel))
            {
                LevelManager.Instance.LoadScene(_firstLevelName);
            }
            
            LevelManager.Instance.LoadScene(lastUnlockedLevel);
        }
    }
}