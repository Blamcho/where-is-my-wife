using TMPro;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        private TextMeshProUGUI _text;
        private string _key;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
            _key = _text.text;
        }

        private void Start()
        {
            LocalizationManager.Instance.OnLanguageChanged += LocalizeText;
            LocalizeText();
        }

        private void OnDestroy()
        {
            LocalizationManager.Instance.OnLanguageChanged -= LocalizeText;
        }

        private void LocalizeText()
        {
            _text.text = _key.Localize();
        }
    }
}
