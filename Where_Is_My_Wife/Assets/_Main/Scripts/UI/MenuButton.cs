using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace WhereIsMyWife.UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        protected Button _button;
        
        private readonly Color _selectedTextColor = Color.white;

        private TextMeshProUGUI _text;
        private Color _originalColor;
        
        protected virtual void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _originalColor = _text.color;
        }

        private void OnDisable()
        {
            ResetColor();
        }

        public void OnSelect(BaseEventData eventData)
        {
            _text.color = _selectedTextColor;
        }

        public void OnDeselect(BaseEventData eventData)
        {   
            ResetColor();
        }
        
        private void ResetColor()
        {
            _text.color = _originalColor;
        }
    }   
}
