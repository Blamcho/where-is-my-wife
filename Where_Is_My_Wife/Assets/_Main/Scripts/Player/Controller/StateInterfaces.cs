using System;
using UnityEngine;

namespace WhereIsMyWife.Controllers
{
    public interface IMovementStateEvents
    {
        Action<float> JumpStart { get; set; }
        Action<float> Run { get; set; }
        Action<float> GravityScale { get; set; }
        Action<float> FallSpeedCap { get; set; }
    }
    
    public interface IPunchingStateEvents
    {
        Action PunchStart { get; set; } 
        Action<float> JumpStart { get; set; }
        Action<float> Run { get; set; }
        Action<float> GravityScale { get; set; }
        Action<float> FallSpeedCap { get; set; }
    }

    public interface IDashStateEvents
    {
        Action<float> Dash { get; set; }
        Action<float> GravityScale { get; set; }
        Action<float> FallSpeedCap { get; set; }
        Action<float> FallingSpeed { get; set; }
    }

    public interface IHookStateEvents
    {
        Action StartQTE { get; set; }
        Action StopQTE { get; set; }
        Action<Vector2> SetPosition { get; set; }
        Action<float> GravityScale { get; set; }
        Action<Vector2> SetVelocity { get; set; }
    }

    public interface IWallHangStateEvents
    {
        Action StartWallHang { get; set; }
        Action<float> WallHangVelocity { get; set; }
        Action<float> WallJumpStart { get; set; }
        Action Turn { get; set; }
    }
    
    public interface IWallJumpStateEvents
    {
        Action<float> WallJumpVelocity { get; set; }
        Action<float> GravityScale { get; set; }
        Action<float> FallSpeedCap { get; set; }
        Action<float> DoubleJump { get; set; }
    }
}