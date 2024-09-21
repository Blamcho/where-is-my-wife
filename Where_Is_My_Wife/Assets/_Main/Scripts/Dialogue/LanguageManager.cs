using Unity.VisualScripting;
using System;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class LanguageManager : Singleton<LanguageManager>
    {
        [field:SerializeField] public Languages Language { get; private set; }

        public Action OnLanguageChanged;

        private void ChangeLanguage(Languages nextLanguages)
        {
            Language = nextLanguages;
            OnLanguageChanged?.Invoke();
        }

        
        
        #if UNITY_EDITOR
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                AlternateLanguage();
            }
        }

        private void AlternateLanguage()
        {
            Language += 1;

            if (Language >= Languages.Max)
            {
                Language = 0;
            }
            
            ChangeLanguage(Language);
        }

        #endif
    }

    public enum Languages
    {
        Spanish,
        English,
        Max,
    }
}
