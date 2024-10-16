using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace WhereIsMyWife.Setting
{
    public class SliderSetting : SettingSelection
    {
        [SerializeField] private Slider _slider;
        [SerializeField] private TextMeshProUGUI _sliderValueText;
        [SerializeField] private int _valueStep;
        [SerializeField] private float _updateInterval;
        private float _updateTimer = 0;
        private int _horizontalValue = 0;

        private void FixedUpdate()
        {
            TickUpdateTimer();
        }

        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            _sliderValueText.color = _selectedTextColor;
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            SetHorizontalValue(0);
        }

        protected override void SubscribeToActions()
        {
            _uiInputEvent.HorizontalStartedAction += SetHorizontalValue;
            _uiInputEvent.HorizontalCanceledAction += SetHorizontalValue;
        }

        protected override void SelectedUnsubscribeFromActions()
        {
            _uiInputEvent.HorizontalStartedAction -= SetHorizontalValue;
            _uiInputEvent.HorizontalCanceledAction -= SetHorizontalValue;
        }

        private void SetHorizontalValue(int value)
        {
            _horizontalValue = value;
        }

        private void UpdateSliderValue()
        {
            _slider.value += _horizontalValue * _valueStep;
            _sliderValueText.text = _slider.value.ToString();
        }
    
        private void TickUpdateTimer()
        {
            _updateTimer += Time.fixedDeltaTime;

            if (_updateTimer >= _updateInterval)
            {
                UpdateSliderValue();
                _updateTimer = 0;
            }
        }

        protected override void ResetColor()
        {
            base.ResetColor();
            _sliderValueText.color = _originalColor;
        }
    }
}
