using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

public interface IHookUIEvents
{
    Action<bool> QTEStateEvent { get; set; }
}

public class HookUIBar : Singleton<HookUIBar>, IHookUIEvents
{
    public IHookUIEvents HookUIEvents => this;

    [SerializeField] private Animator _animatorHookBarQTE;

    private IHookStateEvents _hookStateEvents;
    public Action<bool> QTEStateEvent { get; set; }

    private void Start()
    {
        _hookStateEvents = PlayerManager.Instance.HookStateEvents;
        SubscribeToActions();
    }

    private void OnDestroy()
    {
        UnsubscribeFromActions();
    }

    private void SubscribeToActions()
    {
        _hookStateEvents.StartQTE += StartUIBarAnimation;
        _hookStateEvents.StopQTE += StopUIBarAnimation;
    }

    private void UnsubscribeFromActions()
    {
        _hookStateEvents.StartQTE -= StartUIBarAnimation;
        _hookStateEvents.StopQTE -= StopUIBarAnimation;
    }

    public void OpenQTEWindow()
    {
        SetQTEState(true);
    }

    public void CloseQTEWindow()
    {
        SetQTEState(false);
    }

    public void SetQTEState(bool success)
    {
        QTEStateEvent?.Invoke(success);
    }

    public void StopUIBarAnimation()
    {
        CloseQTEWindow();
        _animatorHookBarQTE.SetBool("executeQTEAnimation", false);
    }

    private void StartUIBarAnimation()
    {
        _animatorHookBarQTE.SetBool("executeQTEAnimation", true);
    }
}