using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
        public class SavedManager : MonoBehaviour
        {
            public static SavedManager Instance { get; private set; } 

            private const string SaveKey = "saved-data";
            private Dictionary<string, object> savedData;

            private void Awake()
            {
                if (Instance != null && Instance != this)
                {
                    Destroy(gameObject); 
                    return;
                }

                Instance = this;
                DontDestroyOnLoad(gameObject); 
            }

            private void Start()
            {
                Load(); 
            }

            public void CompleteLevel(int level)
            {
                if (savedData == null)
                {
                    Debug.LogWarning("Save data not loaded. Cannot complete level.");
                    return;
                }

                SetData("LastLevelFinished", level);
                Save(); 

                Debug.Log($"Level {level} saved as the last finished.");
            }

            private void InitializeSaveData()
            {
                savedData = new Dictionary<string, object>
                {
                    { "LastLevelFinished", 0 }
                };
            }

            private void SetData(string key, object value)
            {
                if (savedData.ContainsKey(key))
                    savedData[key] = value;
                else
                    savedData.Add(key, value);
            }

            private T GetData<T>(string key)
            {
                if (savedData.ContainsKey(key) && savedData[key] is T value)
                    return value;

                return default;
            }

            private void Save()
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
                    savedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);

                    Debug.Log("Game data loaded properly.");
                }
                else
                {
                    InitializeSaveData();
                    Debug.LogWarning("No saved data found. Initialized new data.");
                }
            }
        }
    }
