using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        public event Action OnLanguageChanged;

        [SerializeField]
        private Language _currentLanguage = Language.English;

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
                string[] headers = ParseCSVLine(lines[0]);

                for (int i = 1; i < lines.Length; i++)
                {
                    string[] fields = ParseCSVLine(lines[i]);

                    if (fields.Length > 0)
                    {
                        string key = fields[0];
                        _localizedText[key] = new Dictionary<string, string>();

                        for (int localeIndex = 1; localeIndex < headers.Length; localeIndex++)
                        {
                            string localeCode = headers[localeIndex].Trim();
                            if (localeIndex < fields.Length)
                            {
                                string text = fields[localeIndex].Trim();
                                _localizedText[key][localeCode] = text;
                            }
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("Localization CSV not found.");
            }
        }

        private string[] ParseCSVLine(string csvLine)
        {
            List<string> parsedLine = new List<string>();
            bool isInQuotes = false;
            StringBuilder currentField = new StringBuilder();

            for (int characterIndex = 0; characterIndex < csvLine.Length; characterIndex++)
            {
                char currentCharacter = csvLine[characterIndex];

                if (isInQuotes)
                {
                    if (currentCharacter == '"')
                    {
                        if (CharacterIsEscapedQuote(csvLine, characterIndex))
                        {
                            currentField.Append('"');
                            characterIndex++;
                        }
                        else
                        {
                            isInQuotes = false; 
                        }
                    }
                    else
                    {
                        currentField.Append(currentCharacter);
                    }
                }
                else
                {
                    switch (currentCharacter)
                    {
                        case '"':
                            isInQuotes = true;
                            break;
                        case ',':
                            parsedLine.Add(currentField.ToString());
                            currentField.Clear();
                            break;
                        default:
                            currentField.Append(currentCharacter);
                            break;
                    }
                }
            }

            parsedLine.Add(currentField.ToString());

            return parsedLine.ToArray();
        }

        private static bool CharacterIsEscapedQuote(string csvLine, int characterIndex)
        {
            return characterIndex + 1 < csvLine.Length && csvLine[characterIndex + 1] == '"';
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
