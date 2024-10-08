using UnityEngine;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;

public interface IRunningMethods
{
    public float GetRunAcceleration(float runDirection, float currentVelocityX);
    
    public bool GetIsAccelerating();
}

public class RunningMethods : IRunningMethods
{
    public IRunningMethods Methods => this;
    
    private IPlayerStateIndicator _stateIndicator => PlayerManager.Instance.PlayerStateIndicator;
    
    private IPlayerMovementProperties _movementProperties => PlayerManager.Instance.Properties.Movement;
    private IPlayerJumpProperties _jumpProperties => PlayerManager.Instance.Properties.Jump;
    
    private float _accelerationRate = 0;
    private float _targetSpeed = 0;
    
    public float GetRunAcceleration(float runDirection, float currentVelocityX)
    {
        _targetSpeed = runDirection * _movementProperties.RunMaxSpeed;
        
        UpdateAccelerationRate();

        return GetTargetAndCurrentSpeedDifference(currentVelocityX) * _accelerationRate;
    }
    
    private void UpdateAccelerationRate()
    {
        UpdateBaseAccelerationRate();
        AddJumpHangMultipliers();
    }
    
    private void UpdateBaseAccelerationRate()
    {
        if (_stateIndicator.IsOnGround())
        {
            _accelerationRate = GetGroundAccelerationRate();
        }

        else
        {
            _accelerationRate = GetAirAccelerationRate();
        }
    }
    
    private float GetGroundAccelerationRate()
    {
        if (GetIsAccelerating())
        {
            return _movementProperties.RunAccelerationRate;
        }

        return _movementProperties.RunDecelerationRate;
    }
    
    private float GetAirAccelerationRate()
    {
        if (GetIsAccelerating())
        {
            return _movementProperties.RunAccelerationRate 
                   * _movementProperties.AirAccelerationMultiplier;
        }
            
        return _movementProperties.RunDecelerationRate 
               * _movementProperties.AirDecelerationMultiplier;
    }
    
    public bool GetIsAccelerating()
    {
        return Mathf.Abs(_targetSpeed) > 0.01f;
    }
    
    private void AddJumpHangMultipliers()
    {
        if (_stateIndicator.IsInJumpHang())
        {
            _accelerationRate *= _jumpProperties.HangAccelerationMultiplier;
            _targetSpeed *= _jumpProperties.HangMaxSpeedMultiplier;
        }
    }
    
    private float GetTargetAndCurrentSpeedDifference(float currentVelocityX)
    {
        return _targetSpeed - currentVelocityX;
    }
}
