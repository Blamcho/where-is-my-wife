using System;
using UnityEngine;

namespace WhereIsMyWife.Player.State
{
    public interface IPlayerStateInput
    {
        Action<float> JumpStart { get; set; }
        Action<float> Run { get; set; }
        Action WallHangStart { get; set; }
        Action WallHangEnd { get; set; }
        Action<float> DashStart { get; set; }
        Action<float> GravityScale { get; set; }
        Action<float> FallSpeedCap { get; set; }
        Action Land { get; set; }
        Action HookActivated { get; set; }
        Action<Vector2> ExecuteHookLaunch { get; set; }
        Action<Vector2> HookEnd { get; set; }
    }
}