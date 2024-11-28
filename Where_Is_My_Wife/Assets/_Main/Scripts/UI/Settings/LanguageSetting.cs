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
        [SerializeField] private float _updateInterval = 0.2f;

        private float _updateTimer = 0;
        private int _horizontalValue = 0;
        
        private void FixedUpdate()
        {
            TickUpdateTimer();
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _currentLanguageText.color = _selectedTextColor;
            _leftArrow.color = _selectedTextColor;
            _rightArrow.color = _selectedTextColor;
        }

        protected override void SubscribeToActions()
        {
            _uiInputEvent.HorizontalStartedAction += SetHorizontalValue;
            _uiInputEvent.HorizontalCanceledAction += CancelHorizontalAction;
        }

        protected override void SelectedUnsubscribeFromActions()
        {
            _uiInputEvent.HorizontalStartedAction -= SetHorizontalValue;
            _uiInputEvent.HorizontalCanceledAction -= CancelHorizontalAction;
        }
        
        private void CancelHorizontalAction(int value)
        {
            SetHorizontalValue(value);
            _updateTimer = _updateInterval;
        }
        
        private void SetHorizontalValue(int value)
        {
            _horizontalValue = value;
        }
        
        private void TickUpdateTimer()
        {
            _updateTimer += Time.fixedDeltaTime;

            if (_updateTimer >= _updateInterval && _horizontalValue != 0)
            {
                CycleLanguage(_horizontalValue);
                _updateTimer = 0;
            }
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
