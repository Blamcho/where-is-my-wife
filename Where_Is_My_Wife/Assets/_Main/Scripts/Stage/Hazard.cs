using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;
using Zenject;

public class Hazard : MonoBehaviour
{
    IRespawn _respawn;

    private void Start()
    {
        _respawn = PlayerManager.Instance.Respawn;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _respawn.TriggerRespawn();
        }
    }
}
