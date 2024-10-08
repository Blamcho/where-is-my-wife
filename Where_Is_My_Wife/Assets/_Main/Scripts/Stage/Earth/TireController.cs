using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

public class TireController : MonoBehaviour
{
    [SerializeField] private float _timeToReachYPosition;
    [SerializeField] private float _timeToReachHorizontalPosition;
    [SerializeField] private Ease _horizontalEase = Ease.InSine;

    private Tweener _verticalTweener;
    private Tween _horizontalDistanceTween;
    
    private Vector2 _playerPosition;
    private float _horizontalDistance;
    private bool _isMovingTowardsPlayer;

    private void Start()
    {
        TireManager.Instance.ActivateTireEvent += Activate;
        TireManager.Instance.DeactivateTireEvent += Deactivate;
            
        gameObject.SetActive(false);    
    }

    private void OnDestroy()
    {
        TireManager.Instance.ActivateTireEvent -= Activate;
        TireManager.Instance.DeactivateTireEvent -= Deactivate;
    }

    private void Update()
    {
        if (_isMovingTowardsPlayer)
        {
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        _playerPosition = PlayerManager.Instance.PlayerControllerData.RigidbodyPosition;
    }

    private void Activate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
        StartMovingTowardsPlayer();
    }

    private void Deactivate()
    {
        StopMovingTowardsPlayer();
        gameObject.SetActive(false);
    }

    private void StopMovingTowardsPlayer()
    {
        _isMovingTowardsPlayer = false;
        _verticalTweener.Kill();
        _horizontalDistanceTween.Kill();
    }

    public void StartMovingTowardsPlayer()
    {
        _isMovingTowardsPlayer = true;
        UpdatePlayerPosition();
        
        StartHorizontalDistanceTween();
        StartVerticalTweener();
    }

    private void StartHorizontalDistanceTween()
    {
        _horizontalDistance = transform.position.x - _playerPosition.x;
        
        _horizontalDistanceTween = DOTween
            .To(
                () => _horizontalDistance,
                x => _horizontalDistance = x,
                0,
                _timeToReachHorizontalPosition
            ).SetEase(_horizontalEase);

        _horizontalDistanceTween.OnUpdate(
            delegate() { transform.position = new Vector2(_playerPosition.x + _horizontalDistance, transform.position.y);
        });
    }

    private void StartVerticalTweener()
    {
        _verticalTweener = transform.DOMoveY(_playerPosition.y, _timeToReachYPosition);

        _verticalTweener.OnUpdate(delegate()
        {
            if (Vector3.Distance(transform.position, _playerPosition) > 0.1f)
            {
                _verticalTweener.ChangeEndValue((Vector3)_playerPosition, true);
            }
        });
    }
}
