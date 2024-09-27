using System;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerMovementState : PlayerState, IMovementState, IMovementStateEvents
    {
        public PlayerMovementState() : base(PlayerStateMachine.PlayerState.Movement) { }
        
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
            _playerStateInput.WallHangStart += WallHang;
            _playerStateInput.DashStart += Dash;
            _playerStateInput.HookActivated += Hook;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.JumpStart -= InvokeJumpStart;
            _playerStateInput.Run -= InvokeRun;
            _playerStateInput.GravityScale -= InvokeGravityScale;
            _playerStateInput.FallSpeedCap -= InvokeFallSpeedCap;
            _playerStateInput.WallHangStart -= WallHang;
            _playerStateInput.DashStart -= Dash;
        }

        private void Dash(float _)
        {
            NextState = PlayerStateMachine.PlayerState.Dash;
        }

        private void WallHang()
        {
            NextState = PlayerStateMachine.PlayerState.WallHang;
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

        private void Hook()
        {
            NextState = PlayerStateMachine.PlayerState.Hook;
        }
    }
}