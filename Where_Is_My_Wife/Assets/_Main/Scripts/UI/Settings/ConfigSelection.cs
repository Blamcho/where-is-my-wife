using UnityEngine;
using UnityEngine.EventSystems;
using WhereIsMyWife.Managers;

public class ConfigSelection : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    protected IUIInputEvent _uiInputEvent;

    private void Start()
    {
        _uiInputEvent = InputEventManager.Instance.UIInputEvent;
    }
    
    public virtual void OnSelect(BaseEventData eventData)
    {
        SubscribeToActions();
    }
    
    public virtual void OnDeselect(BaseEventData eventData)
    {
        UnsubscribeFromActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }

    protected virtual void SubscribeToActions() { }
    protected virtual void UnsubscribeFromActions() { }
}
