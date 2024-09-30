using TMPro;
using UnityEngine;

namespace WhereIsMyWife.UI
{
    public class UnityEventText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetText(float value)
        {
            _text.text = value.ToString();
        }
    }
}
