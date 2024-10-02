using UnityEngine;
using UnityEngine.UI;

namespace WhereIsMyWife.Setting  
{
    public class ToggleSetting : MonoBehaviour
    {
        [SerializeField] private Toggle _toggle;

        public void ToggleIsOn()
        {
            _toggle.isOn = !_toggle.isOn;
        }
    }
}
