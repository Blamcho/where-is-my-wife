using UnityEngine;
using UnityEngine.EventSystems;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Setting
{
    public class SettingSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        protected IUIInputEvent _uiInputEvent;

        protected virtual void Start()
        {
            _uiInputEvent = InputEventManager.Instance.UIInputEvent;
        }
    
        public virtual void OnSelect(BaseEventData eventData)
        {
            SubscribeToActions();
        }
    
        public virtual void OnDeselect(BaseEventData eventData)
        {
            SelectedUnsubscribeFromActions();
        }

        protected virtual void OnDestroy()
        {
            SelectedUnsubscribeFromActions();
        }

        protected virtual void SubscribeToActions() { }
        protected virtual void SelectedUnsubscribeFromActions() { }
    }
}
