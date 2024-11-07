using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class DataSaveManager : Singleton<DataSaveManager>
    {
        private const string SaveKey = "saved-data";
        private Dictionary<string, object> savedData;

        public const string LastUnlockedLevelNumberKey = "last-unlocked-level-number";
        public const string LastUnlockedLevelSceneNameKey = "last-unlocked-level-scene-name";
        
        protected override void Awake()
        {
            base.Awake();
            Load(); 
        }   
        
        public void SetData(string key, object value)
        {
            savedData[key] = value;
            Save();
        }

        public T GetData<T>(string key)
        {
            try
            {
                if (savedData.TryGetValue(key, out var value))
                    return (T)Convert.ChangeType(value, typeof(T));
            }
            catch 
            {
                return default;
            }

            return default;
        }

        private void Save()
        {
            string json = JsonConvert.SerializeObject(savedData);
            PlayerPrefs.SetString(SaveKey, json);
        }

        private void Load()
        {
            string json = PlayerPrefs.GetString(SaveKey,null);
            if (json == null || json == "")
            {
                savedData = new Dictionary<string, object>();
                Debug.Log("No save data found. Created new one.");
            }
            else
            {
                savedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json); 
                Debug.Log("Game data loaded properly.");
            }
        }

        public void DeleteSaveData()
        {
            SetData(LastUnlockedLevelNumberKey, 0);
        }

        public void SetLastUnlockedLevel(int levelNumber, string sceneName)
        {
            if (levelNumber > GetData<int>(LastUnlockedLevelNumberKey))
            {
                SetData(LastUnlockedLevelNumberKey, levelNumber);
                SetData(LastUnlockedLevelSceneNameKey, sceneName);
            }
        }
    }
}
