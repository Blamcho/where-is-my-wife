using WhereIsMyWife.Controllers;
using WhereIsMyWife.Player.State;

namespace WhereIsMyWife.Player.StateMachine
{
    /// <summary>
    /// Receives the processed input and decides what to do with it, changes the state and then raises events via different StateEvents interfaces.
    /// </summary>
    public class PlayerStateMachine : StateMachine<PlayerStateMachine.PlayerState>
    {
        public enum PlayerState
        {
            Movement,
            Dash,
            Hook,
            WallHang,
            WallJump,
        }

        public IMovementStateEvents MovementStateEvents { get; private set; }
        public IDashStateEvents DashStateEvents { get; private set; }
        public IWallHangStateEvents WallHangStateEvents { get; private set; }
        public IWallJumpStateEvents WallJumpStateEvents { get; private set; }
        public IHookStateEvents HookStateEvents { get; private set; }
        
        protected void Awake()
        {
            States[PlayerState.Movement] = new PlayerMovementState();
            States[PlayerState.Dash] = new PlayerDashState();
            States[PlayerState.WallHang] = new PlayerWallHangState();
            States[PlayerState.WallJump] = new PlayerWallJumpState();
            States[PlayerState.Hook] = new PlayerHookState();

            MovementStateEvents = (IMovementStateEvents)States[PlayerState.Movement];
            DashStateEvents = (IDashStateEvents)States[PlayerState.Dash];
            WallHangStateEvents = (IWallHangStateEvents)States[PlayerState.WallHang];
            WallJumpStateEvents = (IWallJumpStateEvents)States[PlayerState.WallJump];
            HookStateEvents = (IHookStateEvents)States[PlayerState.Hook];
            
            CurrentState = States[PlayerState.Movement];
        }
    }
}
