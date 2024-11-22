using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.UI
{
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        protected Button _button;
        
        protected readonly Color _selectedTextColor = Color.white;

        protected TextMeshProUGUI _text;
        protected Color _originalColor;
        
        [SerializeField] private bool _shouldPlayClickSound = true;
        
        protected virtual void Awake()
        {
            _button = gameObject.GetComponent<Button>();
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _originalColor = _text.color;
            
            if (_shouldPlayClickSound) _button.onClick.AddListener(PlayClickSound);
        }

        protected virtual void OnDisable()
        {
            ResetColor();
        }

        public virtual void OnSelect(BaseEventData eventData)
        {
            SetSelectedColor();
            PlaySelectedSound();
        }

        public void SetSelectedColor()
        {
            _text.color = _selectedTextColor;
        }
        
        public virtual void OnDeselect(BaseEventData eventData)
        {   
            ResetColor();
        }
        
        protected virtual void ResetColor()
        {
            _text.color = _originalColor;
        }
        
        protected virtual void PlayClickSound()
        {
            AudioManager.Instance.PlaySFX("Click");
        }
        
        private void PlaySelectedSound()
        {
            AudioManager.Instance.PlaySFX("Select");
        }
    }   
}
