using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

public class TireMovement : MonoBehaviour
{
    [FormerlySerializedAs("_timeToReachPlayer")] [SerializeField] private float _timeToReachYPosition;

    private Tweener _verticalTweener;
    
    private Vector2 _playerPosition;
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
        
        StartVerticalTweener();
        
        //TODO: Change movement to lerp from initial position to player position using tweened float
        
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
