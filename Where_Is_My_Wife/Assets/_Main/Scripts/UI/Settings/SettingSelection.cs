using UnityEngine;
using UnityEngine.EventSystems;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Setting
{
    public class SettingSelection : MenuButton, ISelectHandler, IDeselectHandler
    {
        protected IUIInputEvent _uiInputEvent;

        protected virtual void Start()
        {
            _uiInputEvent = InputEventManager.Instance.UIInputEvent;
        }
    
        public virtual void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SubscribeToActions();
        }
    
        public virtual void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            SelectedUnsubscribeFromActions();
        }

        protected virtual void OnDisable()
        {
            SelectedUnsubscribeFromActions();
        }

        protected virtual void SubscribeToActions() { }
        protected virtual void SelectedUnsubscribeFromActions() { }
    }
}
