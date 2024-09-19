using System;
using UniRx;
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

    public interface IDashStateEvents
    {
        Action<Vector2> Dash { get; set; }
    }

    public interface IHookStateEvents
    {
        
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
    }
}