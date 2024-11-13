using UnityEngine.EventSystems;
using WhereIsMyWife.Managers;
using WhereIsMyWife.UI;

namespace WhereIsMyWife.Setting
{
    public class SettingSelection : MenuButton
    {
        protected IUIInputEvent _uiInputEvent;

        protected virtual void Start()
        {
            _uiInputEvent = InputEventManager.Instance.UIInputEvent;
        }
    
        public override void OnSelect(BaseEventData eventData)
        {
            base.OnSelect(eventData);
            SubscribeToActions();
        }
    
        public override void OnDeselect(BaseEventData eventData)
        {
            base.OnDeselect(eventData);
            SelectedUnsubscribeFromActions();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            SelectedUnsubscribeFromActions();
        }

        protected virtual void SubscribeToActions() { }
        protected virtual void SelectedUnsubscribeFromActions() { }
    }
}
