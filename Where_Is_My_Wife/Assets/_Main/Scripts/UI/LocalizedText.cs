using TMPro;
using UnityEngine;

namespace WhereIsMyWife.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        private TextMeshProUGUI _text;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {
            _text.text = _text.text.Localize();
        }
    }
}
