using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.StateMachine;

namespace WhereIsMyWife.Player.State
{
    public class PlayerHookState : PlayerState, IHookState, IHookStateEvents
    {
        public PlayerHookState() : base(PlayerStateMachine.PlayerState.Hook) { }
        public Action<Vector2> AddImpulse { get; set; }
        public Action<Vector2> SetPosition { get; set; }
        public Action<float> GravityScale { get; set; }

        protected override void SubscribeToObservables()
        {
            _playerStateInput.WallHangStart += WallHangStart;
            _playerStateInput.Land += MovementStart;
        }

        protected override void UnsubscribeToObservables()
        {
            _playerStateInput.WallHangStart -= WallHangStart;
            _playerStateInput.Land -= MovementStart;
        }

        public override void EnterState()
        {
            base.EnterState();
            AddImpulse?.Invoke(_playerStateIndicator.HookLaunchImpulse);
            ExecuteHookEnd().Forget();
        }

        public override void ExitState()
        {
            base.ExitState();
        }

        private async UniTaskVoid ExecuteHookEnd()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.25f));
            NextState = PlayerStateMachine.PlayerState.Movement;
        }

        private void WallHangStart()
        {
            NextState = PlayerStateMachine.PlayerState.WallHang;
        }

        private void MovementStart()
        {
            NextState = PlayerStateMachine.PlayerState.Movement;
        }
    }
}