using System;
using System.Collections.Generic;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        public event Action OnLanguageChanged;
        
        [SerializeField] private Language _currentLanguage = Language.English;
        
        private Dictionary<string, Dictionary<string, string>> _localizedText;
        private Dictionary<Language, string> _localeCode;
        private string _currentLocaleCode;

        public enum Language
        {
            English,
            Spanish,
            BrazilianPortuguese,
            French,
            Max,
        }
        
        protected override void Awake()
        {
            base.Awake();
            SetLanguageKeys();
            _currentLocaleCode = _localeCode[_currentLanguage];
            LoadLocalizationCSV();
        }

        private void SetLanguageKeys()
        {
            _localeCode = new Dictionary<Language, string>();
            
            _localeCode[Language.English] = "en";
            _localeCode[Language.Spanish] = "es";
            _localeCode[Language.BrazilianPortuguese] = "pt-BR";
            _localeCode[Language.French] = "fr";
        }

        private void LoadLocalizationCSV()
        {
            _localizedText = new Dictionary<string, Dictionary<string, string>>();
            TextAsset localizationFile = Resources.Load<TextAsset>("Localization"); 

            if (localizationFile != null)
            {
                string[] lines = localizationFile.text.Split('\n');
                string[] headers = lines[0].Split(',');

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = lines[i].Split(',');

                    if (fields.Length > 0)
                    {
                        string key = fields[0];
                        _localizedText[key] = new Dictionary<string, string>();

                        for (int j = 1; j < headers.Length; j++)
                        {
                            string lang = headers[j].Trim();
                            string text = fields[j].Trim();
                            _localizedText[key][lang] = text;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Localization CSV not found.");
            }
        }

        public string GetLocalizedValue(string key)
        {
            if (_localizedText.ContainsKey(key) && _localizedText[key].ContainsKey(_currentLocaleCode))
            {
                return _localizedText[key][_currentLocaleCode];
            }

            Debug.LogWarning($"Localization key '{key}' not found for language '{_currentLocaleCode}'.");
            return "#ERROR";
        }

        public void CycleLanguage(int direction)
        {
            _currentLanguage += direction;
            
            if (_currentLanguage < 0)
            {
                _currentLanguage = Language.Max - 1; 
            }
            else if (_currentLanguage >= Language.Max)
            {
                _currentLanguage = 0;
            }
            
            RefreshLanguage();
        }
        
        private void RefreshLanguage()
        {
            _currentLocaleCode = _localeCode[_currentLanguage];
            OnLanguageChanged?.Invoke();
        }
    }
}
