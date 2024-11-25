using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Setting  
{
    public class ToggleSetting : MenuButton
    {
        [SerializeField] private SettingType _settingType;
        [SerializeField] private Toggle _toggle;

        protected override void Awake()
        {
            base.Awake();
            _button.onClick.AddListener(ToggleState);
        }

        private void Start()
        {
            switch (_settingType)
            {
                case SettingType.Fullscreen:
                    _toggle.isOn = GameManager.Instance.IsFullscreen;
                    break;
            }
        }

        private void ToggleState()
        {
            _toggle.isOn = !_toggle.isOn;
            
            switch (_settingType)
            {
                case SettingType.Fullscreen:
                    GameManager.Instance.SetFullscreen(_toggle.isOn);
                    break;
            }
        }
        
        private enum SettingType
        {
            Fullscreen
        }
    }
}
