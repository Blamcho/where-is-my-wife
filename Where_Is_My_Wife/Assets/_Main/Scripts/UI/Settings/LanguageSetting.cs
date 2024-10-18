using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Setting
{
    public class LanguageSetting : SettingSelection
    {
        [SerializeField] private TextMeshProUGUI _currentLanguageText;
        [SerializeField] private Image _leftArrow;
        [SerializeField] private Image _rightArrow;

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _currentLanguageText.color = _selectedTextColor;
            _leftArrow.color = _selectedTextColor;
            _rightArrow.color = _selectedTextColor;
        }

        protected override void SubscribeToActions()
        {
            _uiInputEvent.HorizontalStartedAction += CycleLanguage;
        }

        protected override void SelectedUnsubscribeFromActions()
        {
            _uiInputEvent.HorizontalStartedAction -= CycleLanguage;
        }

        private void CycleLanguage(int value)
        {
            LocalizationManager.Instance.CycleLanguage(value);
        }
        
        protected override void ResetColor()
        {
            base.ResetColor();
            _currentLanguageText.color = _originalColor;
            _leftArrow.color = _originalColor;
            _rightArrow.color = _originalColor;
        }
    }
}
