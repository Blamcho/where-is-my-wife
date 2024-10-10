using System;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public interface IPlayerInputEvent
    {
        Action JumpStartAction { get; set; }
        Action JumpEndAction { get; set; }
        Action<float> RunAction { get; set; }
        Action<float> DashAction { get; set; }
        Action<Vector2> UseItemAction { get; set; }
        Action HookStartAction { get; set; }
        Action HookEndAction { get; set; }
        Action LookUpAction { get; set; }
        Action<bool> LookDownAction { get; set; }
        Action PunchAction { get; set; }
    }
    
    public interface IUIInputEvent
    {
        Action<int> HorizontalStartedAction { get; set; }
        Action<int> HorizontalCanceledAction { get; set; }
        Action SubmitStartAction { get; set; }
        Action CancelStartAction { get; set; }
        Action PauseStartAction { get; set; }
    }
    
    public enum ControllerType
    {
        Keyboard,
        Xbox,
        Playstation,
        Nintendo,
    }
}