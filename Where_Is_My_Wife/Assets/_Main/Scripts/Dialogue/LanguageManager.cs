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
                CycleThroughLanguages(1);
            }
        }

        public void CycleThroughLanguages(int direction)
        {
            Language += direction;

            if (Language >= Languages.Max)
            {
                Language = 0;
            }

            if (Language < 0)
            {
                Language = Languages.Max - 1;
            }
            
            ChangeLanguage(Language);
        }

        #endif
    }

    public enum Languages
    {
        English,
        Spanish,
        Max,
    }
}
