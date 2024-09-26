using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerHookState : PlayerState, IMovementState, IMovementStateEvents
    {
        public PlayerHookState() : base(PlayerStateMachine.PlayerState.Hook) { }

        public Action<float> JumpStart { get; set; }
        public Action<float> Run { get; set; }
        public Action<float> GravityScale { get; set; }
        public Action<float> FallSpeedCap { get; set; }
        //public Action<float> ThrustForce { get; set; }

        protected override void SubscribeToObservables()
        {

        }
    }
}