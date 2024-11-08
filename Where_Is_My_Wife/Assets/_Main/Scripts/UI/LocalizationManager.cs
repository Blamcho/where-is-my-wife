using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class LocalizationManager : Singleton<LocalizationManager>
    {
        public event Action OnLanguageChanged;

        private readonly Dictionary<string, Dictionary<string, string>> LocalizationDatabase = new ();

        private readonly string[] LanguageCodes =
        {
            "en",
            "es",
            "pt-BR",
            "fr"
        };

        private int _currentLanguageIndex = 0;

#if UNITY_EDITOR 

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                CycleLanguage(1);
            }
        }

#endif
        
        protected override void Awake()
        {
            base.Awake();
            //TODO: Set current language from save data
            LoadLocalizationCSV();
        }
        
        private void LoadLocalizationCSV()
        {
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
                        LocalizationDatabase[key] = new Dictionary<string, string>();

                        for (int localeIndex = 1; localeIndex < headers.Length; localeIndex++)
                        {
                            string localeCode = headers[localeIndex].Trim();
                            if (localeIndex < fields.Length)
                            {
                                string text = fields[localeIndex].Trim();
                                LocalizationDatabase[key][localeCode] = text;
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
            string currentLocaleCode = LanguageCodes[_currentLanguageIndex];
            
            if (LocalizationDatabase.ContainsKey(key) && LocalizationDatabase[key].ContainsKey(currentLocaleCode))
            {
                return LocalizationDatabase[key][currentLocaleCode];
            }

            Debug.LogWarning($"Localization key '{key}' not found for language '{currentLocaleCode}'.");
            return "#ERROR";
        }

        public void CycleLanguage(int direction)
        {
            _currentLanguageIndex += direction;

            if (_currentLanguageIndex < 0)
            {
                _currentLanguageIndex = LanguageCodes.Length - 1;
            }
            else if (_currentLanguageIndex >= LanguageCodes.Length)
            {
                _currentLanguageIndex = 0;
            }

            OnLanguageChanged?.Invoke();
        }
    }
}
