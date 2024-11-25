using System;
using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Setting  
{
    public class ToggleSetting : MenuButton
    {
        [SerializeField] private Toggle _toggle;

        public event Action<bool> OnValueChanged; 
        
        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(ToggleState);
        }

        private void ToggleState()
        {
            _toggle.isOn = !_toggle.isOn;
            OnValueChanged?.Invoke(_toggle.isOn);
        }
        
        public void SetToggleState(bool state)
        {
            _toggle.isOn = state;
        }
    }
}
