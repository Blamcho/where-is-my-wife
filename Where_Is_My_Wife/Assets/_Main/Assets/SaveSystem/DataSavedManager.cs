using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class DataSaveManager : Singleton<DataSaveManager>
    {
        private const string SaveKey = "saved-data";
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
            if (savedData.ContainsKey(key) && savedData[key] is T value)
                return value;

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
            if (json == null)
            {
                savedData = new Dictionary<string, object>();
            }
            else
            {
                savedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(json); 
            }
            Debug.Log("Game data loaded properly.");
        }
    }
}
