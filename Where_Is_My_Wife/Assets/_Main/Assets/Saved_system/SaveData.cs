using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.SavedData
{
    public class SavedData : MonoBehaviour
    {
        [System.Serializable]
        public class SavedDataInfo
        {
            public int LastLevelFinished { get; private set; }
            public Dictionary<string, object> Settings { get; private set; }
            public List<int> CompletedLevels { get; private set; }

            [JsonConstructor]
            public SavedDataInfo(int lastLevelFinished, Dictionary<string, object> settings, List<int> completedLevels)
            {
                LastLevelFinished = lastLevelFinished;
                Settings = settings ?? new Dictionary<string, object>();
                CompletedLevels = completedLevels ?? new List<int>();
            }

            public void SetLastLevel(int level) => LastLevelFinished = level;

            public void AddCompletedLevel(int level)
            {
                if (!CompletedLevels.Contains(level))
                {
                    CompletedLevels.Add(level);
                    Debug.Log($"Level {level} marked as completed.");
                }
            }
        }

        private const string SaveKey = "saved-data";
        private SavedDataInfo savedData;

        [Header("Save System Buttons")] [SerializeField]
        private Button saveButton;

        [SerializeField] private Button loadButton;
        [SerializeField] private Button deleteButton;

        private void Start()
        {
            Load();
            AssignButtonListeners();
        }

        private void AssignButtonListeners()
        {
            saveButton?.onClick.AddListener(Save);
            loadButton?.onClick.AddListener(Load);
            deleteButton?.onClick.AddListener(DeleteSave);
        }

        public void CompleteLevel(int level)
        {
            if (savedData == null)
            {
                savedData = new SavedDataInfo(level, new Dictionary<string, object>(), new List<int>());
            }
            else
            {
                savedData.SetLastLevel(level);
                savedData.AddCompletedLevel(level);
            }

            Save();
            Debug.Log($"Level {level} saved.");
        }

        public void Save()
        {
            string json = JsonConvert.SerializeObject(savedData);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
            Debug.Log("Game data saved.");
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                string json = PlayerPrefs.GetString(SaveKey);
                savedData = JsonConvert.DeserializeObject<SavedDataInfo>(json);

                Debug.Log($"Game data loaded: Last level finished: {savedData.LastLevelFinished}");
                Debug.Log($"Completed Levels: {string.Join(", ", savedData.CompletedLevels)}");
            }
            else
            {
                savedData = new SavedDataInfo(0, new Dictionary<string, object>(), new List<int>());
                Debug.LogWarning("No saved data found.");
            }
        }

        public void DeleteSave()
        {
            if (PlayerPrefs.HasKey(SaveKey))
            {
                PlayerPrefs.DeleteKey(SaveKey);
                savedData = new SavedDataInfo(0, new Dictionary<string, object>(), new List<int>());
                Debug.Log("Saved data deleted.");
            }
            else
            {
                Debug.LogWarning("No saved data to delete.");
            }
        }
    }
}
