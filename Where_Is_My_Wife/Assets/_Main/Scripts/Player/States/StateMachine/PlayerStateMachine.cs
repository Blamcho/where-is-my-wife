using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Player.StateMachine
{
    public class PlayerStateMachine : StateManager<PlayerStateMachine.PlayerState>
    {
        public enum PlayerState
        {
            Movement,
            Dash,
            Hook,
            WallHang,
            WallJump,
        }
        private IMovementState _movementState;
        private IDashState _dashState;
        private IWallHangState _wallHangState;
        private IWallJumpState _wallJumpState;

        protected override void Start()
        {
            _movementState = PlayerManager.Instance.MovementState;
            _dashState = PlayerManager.Instance.DashState;
            _wallHangState = PlayerManager.Instance.WallHangState;
            _wallJumpState = PlayerManager.Instance.WallJumpState;
            
            States[PlayerState.Movement] = _movementState;
            States[PlayerState.Dash] = _dashState;
            States[PlayerState.WallHang] = _wallHangState;
            States[PlayerState.WallJump] = _wallJumpState;
            
            CurrentState = States[PlayerState.Movement];
            
            base.Start();
        }
    }
}
