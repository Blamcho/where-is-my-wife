using System;
using TMPro;
using UnityEngine;
using WhereIsMyWife.Managers;

public class MultilingualText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private string _englishText;
    [SerializeField] private string _spanishText;
    
    private LanguageManager _languageManager;

    private void Start()
    {
        _languageManager = LanguageManager.Instance;
        _languageManager.OnLanguageChanged += RefreshText;
        RefreshText();
    }

    private void OnDestroy()
    {
        _languageManager.OnLanguageChanged -= RefreshText;
    }

    private void RefreshText()
    {
        switch (_languageManager.Language)
        {
            case Languages.English:
                _text.text = _englishText;
                break;
            case Languages.Spanish:
                _text.text = _spanishText;
                break;
        }
    }
}
