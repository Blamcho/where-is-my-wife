using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhereIsMyWife.Managers;

public class TireManager : SingletonDestroyOnLoad<TireManager>
{
    public event Action<Vector2> ActivateTireEvent;
    public event Action DeactivateTireEvent;

    private void Start()
    {
        PlayerManager.Instance.RespawnAction += DeactivateTire;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.RespawnAction -= DeactivateTire;
    }

    public void ActivateTire(Vector2 tirePosition)
    {
        ActivateTireEvent?.Invoke(tirePosition);
    }

    public void DeactivateTire()
    {
        DeactivateTireEvent?.Invoke();
    }
    
    public void DeactivateTire(Vector3 _)
    {
        DeactivateTireEvent?.Invoke();
    }
}
