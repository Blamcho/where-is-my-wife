using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerPunchingState : PlayerState, IPunchingState, IPunchingStateEvents
    {
        public PlayerPunchingState() : base(PlayerStateMachine.PlayerState.Punching) { }

        public Action PunchStart { get; set; }
        public Action<float> JumpStart { get; set; }
        public Action<float> Run { get; set; }
        public Action<float> GravityScale { get; set; }
        public Action<float> FallSpeedCap { get; set; }
        
        protected override void SubscribeToObservables()
        {
            _playerStateInput.JumpStart += InvokeJumpStart;
            _playerStateInput.Run += InvokeRun;
            _playerStateInput.GravityScale += InvokeGravityScale;
            _playerStateInput.FallSpeedCap += InvokeFallSpeedCap;
            _playerStateInput.PunchEnd += EndPunch;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.JumpStart -= InvokeJumpStart;
            _playerStateInput.Run -= InvokeRun;
            _playerStateInput.GravityScale -= InvokeGravityScale;
            _playerStateInput.FallSpeedCap -= InvokeFallSpeedCap;
            _playerStateInput.PunchEnd -= EndPunch;
        }

        public override void EnterState()
        {
            base.EnterState();
            PunchStart?.Invoke();
        }

        private void InvokeJumpStart(float jumpForce)
        {
            JumpStart?.Invoke(jumpForce);
        }

        private void InvokeRun(float runAcceleration)
        {
            Run?.Invoke(runAcceleration);
        }

        private void InvokeGravityScale(float gravityScale)
        {
            GravityScale?.Invoke(gravityScale);
        }

        private void InvokeFallSpeedCap(float fallSpeedCap)
        {
            FallSpeedCap?.Invoke(fallSpeedCap);
        }

        private void EndPunch()
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }
}