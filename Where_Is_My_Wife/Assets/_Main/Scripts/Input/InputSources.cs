using System;
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

    public interface ISpecialInputEvent
    {
        Action PauseAction { get; set; }
    }

    public interface IUIInputEvent
    {
        Action<int> HorizontalStartedAction { get; set; }
        Action SubmitStartAction { get; set; }
        Action CancelStartAction { get; set; }
    }
    
    public enum ControllerType
    {
        Keyboard,
        Xbox,
        Playstation,
        Nintendo,
    }
}