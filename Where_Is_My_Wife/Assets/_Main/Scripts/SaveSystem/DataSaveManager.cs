using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class DataSaveManager : Singleton<DataSaveManager>
    {
        private const string SaveKey = "saved-data";
        public const string LastUnlockedLevelNumberKey = "last-unlocked-level-number";
        public const string LastPlayedStoryModeLevelSceneNameKey = "last-played-story-mode-level-scene-name";
        public const string LastPlayedStoryModeLevelIndexKey = "last-played-story-mode-level-index";
        public const string MusicVolumeKey = "music-volume";
        public const string SfxVolumeKey = "sfx-volume";
        public const string LanguageIndexKey = "language";
        public const string FullscreenKey = "fullscreen";
        
        private Dictionary<string, object> savedData;

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
            string json = PlayerPrefs.GetString(SaveKey, null);
            if (json == null || json == "")
            {
                savedData = new Dictionary<string, object>();
                SetDefaultData();
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
            SetData(LastPlayedStoryModeLevelSceneNameKey, null);
            SetData(LastPlayedStoryModeLevelIndexKey, 0);
        }

        public void SetNextLevelParameters(int levelNumber, string sceneName, bool isInStoryMode)
        {
            if (levelNumber > GetData<int>(LastUnlockedLevelNumberKey))
            {
                SetData(LastUnlockedLevelNumberKey, levelNumber);
            }

            if (isInStoryMode)
            {
                SetData(LastPlayedStoryModeLevelIndexKey, levelNumber);
                SetData(LastPlayedStoryModeLevelSceneNameKey, sceneName);
            }
        }
        
        private void SetDefaultData()
        {
            SetData(LastUnlockedLevelNumberKey, 0);
            SetData(LastPlayedStoryModeLevelSceneNameKey, null);
            SetData(LastPlayedStoryModeLevelIndexKey, 0);
            SetData(MusicVolumeKey, 1f);
            SetData(SfxVolumeKey, 1f);
            SetData(LanguageIndexKey, 0);
            SetData(FullscreenKey, true);
        }
    }
}
