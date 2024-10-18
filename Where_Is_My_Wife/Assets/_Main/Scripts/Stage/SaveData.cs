using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class SavedData : MonoBehaviour
{
    [SerializeField]
    public class SavedDataData
    {
        public int LastLevelFinished { get; set; }
        public Dictionary<string, object> Sttings { get; set; }

        [JsonConstructor]
        public SavedDataData(int lastLevelFinished, Dictionary<string, object> sttings)
        {
            LastLevelFinished = lastLevelFinished;
            Sttings = sttings;
        }

        private void NewLevelFinished(int level)
        {
             //savedData.lastLevelFinished = level;
            Save();
        }

        private void Save()
        {
            //PlayerPrefs.setkey("saved-data", JsonConvert.Serialize(SavedData));
        }

        private void Load()
        {
           // savedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(PlayerPrefs.GetString("saved-data"));
        }
    }
}