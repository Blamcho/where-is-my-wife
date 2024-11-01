using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;
using Random = UnityEngine.Random;

public class BossController : MonoBehaviour
{
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _minProjectileSpawnInterval = 1f;
    [SerializeField] private float _maxProjectileSpawnInterval = 3f;
    
    private Sequence _firingSequence;
    
    private void Start()
    {
        BossManager.Instance.StartFiringEvent += StartFiring;
        BossManager.Instance.StopFiringEvent += StopFiring;
    }

    private void OnDestroy()
    {
        BossManager.Instance.StartFiringEvent -= StartFiring;
        BossManager.Instance.StopFiringEvent -= StopFiring;
    }

    private void StartFiring()
    {
        _firingSequence = DOTween.Sequence();

        _firingSequence.AppendInterval(Random.Range(_minProjectileSpawnInterval, _maxProjectileSpawnInterval));
        
        _firingSequence.AppendCallback(() =>
        {
            Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        });

        _firingSequence.SetLoops(-1, LoopType.Restart);
    }

    private void StopFiring()
    {
        _firingSequence.Kill();
    }
}
