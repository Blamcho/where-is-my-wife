using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Config
{
    public class ToggleConfig : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        public void ToggleIsOn()
        {
            _toggle.isOn = !_toggle.isOn;
        }
    }
}
