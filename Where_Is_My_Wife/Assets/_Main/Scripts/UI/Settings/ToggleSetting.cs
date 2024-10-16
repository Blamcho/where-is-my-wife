using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Setting  
{
    public class ToggleSetting : MenuButton
    {
        [SerializeField] private Toggle _toggle;

        public void ToggleIsOn()
        {
            _toggle.isOn = !_toggle.isOn;
        }
    }
}
