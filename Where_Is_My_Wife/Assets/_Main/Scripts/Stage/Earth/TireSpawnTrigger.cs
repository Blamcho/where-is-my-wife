using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WhereIsMyWife.Managers;

public class TireSpawnTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _tirePrefab;
    [SerializeField] private Transform _spawnTransform;
    
    private bool _hasBeenTriggeredAlready;

    private void Start()
    {
        PlayerManager.Instance.RespawnAction += Reset;
    }

    private void Reset(Vector3 _)
    {
        _hasBeenTriggeredAlready = false;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasBeenTriggeredAlready)
        {
            _hasBeenTriggeredAlready = true;
            TireManager.Instance.ActivateTire(_spawnTransform.position);
        }
    }
}
