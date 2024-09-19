using System;
using UniRx;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public interface IPlayerInputEvent
    {
        Action JumpStartAction { get; set; }
        Action JumpEndAction { get; set; }
        Action<float> RunAction { get; set; }
        Action<Vector2> DashAction { get; set; }
        Action<Vector2> UseItemAction { get; set; }
        Action HookStartAction { get; set; }
        Action HookEndAction { get; set; }
        Action LookUpAction { get; set; }
        Action<bool> LookDownAction { get; set; }
    }
    
    public enum ControllerType
    {
        Keyboard,
        Xbox,
        Playstation,
        Nintendo,
    }
}