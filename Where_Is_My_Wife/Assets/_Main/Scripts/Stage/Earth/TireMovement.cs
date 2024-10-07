using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

public class TireMovement : MonoBehaviour
{
    [SerializeField] private float _timeToReachPlayer;

    private Tweener _tweener;
    
    private Vector2 _playerPosition;
    private bool _isMovingTowardsPlayer;

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

    public void StartMovingTowardsPlayer()
    {
        UpdatePlayerPosition();
        _isMovingTowardsPlayer = true;
        
        //TODO: Change movement to lerp from initial position to player position using tweened float
        _tweener = transform.DOMove(_playerPosition, _timeToReachPlayer);
        
        _tweener.OnUpdate(delegate()
        {
            if (Vector3.Distance(transform.position, _playerPosition) > 0.1f)
            {
                _tweener.ChangeEndValue((Vector3)_playerPosition, true);
            }
        });
    }
}
