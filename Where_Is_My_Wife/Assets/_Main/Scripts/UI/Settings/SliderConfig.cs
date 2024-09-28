using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderConfig : ConfigSelection
{
    [SerializeField] private Slider _slider;
    [SerializeField] private int _valueStep;
    
    protected override void SubscribeToActions()
    {
        _uiInputEvent.HorizontalStartedAction += UpdateSliderValue;
    }

    protected override void UnsubscribeFromActions()
    {
        _uiInputEvent.HorizontalStartedAction -= UpdateSliderValue;
    }

    private void UpdateSliderValue(int value)
    {
        _slider.value += value * _valueStep;
    }
}
