using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

public class HookUIBar : Singleton<HookUIBar>
{
    [SerializeField] private Animator _animatorHookBarQTE;

    public Action<bool> QTEWindowUpdated { get; set; }
    public Action FailedQTE { get; set; }
    private IHookStateEvents _hookStateEvents { get; set; }
    //public Action EndUIHookBarAnimation;

    private void Start()
    {
        _hookStateEvents = PlayerManager.Instance.HookStateEvents;
        Subscription();
    }
    private void OnDestroy()
    {
        Unsubscription();
    }

    private void Subscription()
    {
        _hookStateEvents.StartHook += StartHookAnimation;
        _hookStateEvents.ExecuteHook += ExecuteHookLaunch;
    }

    private void Unsubscription()
    {
        _hookStateEvents.StartHook -= StartHookAnimation;
        _hookStateEvents.ExecuteHook -= ExecuteHookLaunch;
    }

    public void OpenQTEWindow()
    {
        QTEWindowUpdated?.Invoke(true);
    }

    public void CloseQTEWindow()
    {
        QTEWindowUpdated?.Invoke(false);
    }

    public void PlayerFailedQTE()
    {
        FailedQTE?.Invoke();
    }

    public void EndUIBarAnimation()
    {
        //EndUIHookBarAnimation.Invoke();
        _animatorHookBarQTE.SetBool("executeQTEAnimation", false);
    }

    private void StartHookAnimation()
    {
        _animatorHookBarQTE.SetBool("executeQTEAnimation", true);
    }

    private void ExecuteHookLaunch(Vector2 hookPosition)
    {
        //Dissolve Animation
    }
}