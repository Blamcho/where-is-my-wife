using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

public class TireMovement : MonoBehaviour
{
    [SerializeField] private float _timeToReachYPosition;
    [SerializeField] private float _timeToReachHorizontalPosition;

    private Tweener _verticalTweener;
    private Tween _horizontalDistanceTween;
    
    private Vector2 _playerPosition;
    private float _horizontalDistance;
    private bool _isMovingTowardsPlayer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // TODO: Change to trigger invoke
        {
            StartMovingTowardsPlayer();
        }
        
        if (_isMovingTowardsPlayer)
        {
            UpdatePlayerPosition();
        }
    }

    private void UpdatePlayerPosition()
    {
        _playerPosition = PlayerManager.Instance.PlayerControllerData.RigidbodyPosition;
    }

    public void StartMovingTowardsPlayer()
    {
        UpdatePlayerPosition();
        _isMovingTowardsPlayer = true;
        
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
            );

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
