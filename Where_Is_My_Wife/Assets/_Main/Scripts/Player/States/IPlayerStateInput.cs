using System;
using UnityEngine;
using UnityEngine.UIElements;

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
        Action HookStart { get; set; }
        Action PunchStart { get; set; }
        Action PunchEnd { get; set; }
    }
}