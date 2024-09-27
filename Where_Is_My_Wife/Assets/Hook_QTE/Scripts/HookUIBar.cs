using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

public class HookUIBar : Singleton<HookUIBar>
{
    [SerializeField] private Animator _animatorHookBarQTE;

    private IHookStateEvents _hookStateEvents;
    public Action<bool> QTEWindowUpdated { get; set; }
    public Action FailedQTE { get; set; }
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
        _hookStateEvents.HookQTEEnd += EndQTE;
    }

    private void Unsubscription()
    {
        _hookStateEvents.StartHook -= StartHookAnimation;
        _hookStateEvents.ExecuteHook -= ExecuteHookLaunch;
        _hookStateEvents.HookQTEEnd -= EndQTE;
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
        Debug.Log("Starting QTE Animation...");
        _animatorHookBarQTE.SetBool("executeQTEAnimation", true);
    }

    private void ExecuteHookLaunch(Vector2 hookPosition)
    {
        EndUIBarAnimation();
    }

    private void EndQTE(Vector2 originalVelocity)
    {
        EndUIBarAnimation();
    }
}