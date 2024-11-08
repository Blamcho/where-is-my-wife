using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
    /// <summary>
    /// This is independent to the <see cref="PlayerController"/> and just reacts to the Events to show the animations.
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IMovementStateEvents _movementStateEvents;
        private IWallHangStateEvents _wallHangStateEvents;
        private IDashStateEvents _dashStateEvents;
        private IPunchingStateEvents _punchingStateEvents;
        private IHookStateEvents _hookStateEvents;
        
        private IPlayerStateIndicator _playerStateIndicator;
        
        private const string RUN_ANIMATION_STATE = "walk";
        private const string IDLE_ANIMATION_STATE = "idle";
        private const string FALL_ANIMATION_STATE = "fall";
        private const string JUMP_ANIMATION_STATE = "jump";
        private const string CROUCH_ANIMATION_STATE = "crouch";
        private const string WALL_SLIDE_ANIMATION_STATE = "wall_slide";
        private const string LAND_ANIMATION_STATE = "land";
        private const string WALL_HIT_ANIMATION_STATE = "wall_hit";
        private const string FORWARD_SMASH_STATE = "f_smash";

        private string _currentAnimationState = "";

        private void Start()
        {
            _playerStateIndicator = PlayerManager.Instance.PlayerStateIndicator;
            
            _movementStateEvents = PlayerManager.Instance.MovementStateEvents;
            _wallHangStateEvents = PlayerManager.Instance.WallHangStateEvents;
            _dashStateEvents = PlayerManager.Instance.DashStateEvents;
            _punchingStateEvents = PlayerManager.Instance.PunchingStateEvents;
            _hookStateEvents = PlayerManager.Instance.HookStateEvents;
            
            SubscribeToStateEvents();

            PlayerManager.Instance.DeathAction += Die;
            PlayerManager.Instance.RespawnCompleteAction += CompleteRespawn;
        }

        private void OnDestroy()
        {
            UnsubscribeFromStateEvents();
            
            PlayerManager.Instance.DeathAction -= Die;
            PlayerManager.Instance.RespawnCompleteAction -= CompleteRespawn;
        }

        private void SubscribeToStateEvents()
        {
            _movementStateEvents.JumpStart += Jump;
            _movementStateEvents.Run += Run;

            _dashStateEvents.DashStart += Dash;

            _wallHangStateEvents.StartWallHang += StartWallHang;
            _wallHangStateEvents.WallJumpStart += Fall;

            _hookStateEvents.HookStart += Hook;
            
            _punchingStateEvents.PunchStart += Punch;
        }
        
        private void UnsubscribeFromStateEvents()
        {
            _movementStateEvents.JumpStart -= Jump;
            _movementStateEvents.Run -= Run;
            
            _dashStateEvents.DashStart -= Dash;

            _wallHangStateEvents.StartWallHang -= StartWallHang;
            _wallHangStateEvents.WallJumpStart -= Fall;
            
            _hookStateEvents.HookStart -= Hook;
            
            _punchingStateEvents.PunchStart -= Punch;
        }
        
        private void Jump(float _)
        {
            PlayAnimationState(JUMP_ANIMATION_STATE);
        }

        private void Fall(float _)
        {
            PlayAnimationState(FALL_ANIMATION_STATE);
        }
        
        private void Run(float _)
        {
            if (!_playerStateIndicator.IsJumping)
            {
                if (_playerStateIndicator.IsRunFalling)
                {
                    PlayAnimationState(FALL_ANIMATION_STATE);
                }
                else if (_playerStateIndicator.IsAccelerating)
                {
                    PlayAnimationState(RUN_ANIMATION_STATE);
                }
                else
                {
                    if (_playerStateIndicator.IsLookingDown)
                    {
                        PlayAnimationState(CROUCH_ANIMATION_STATE);
                    }
                    else
                    {
                        PlayAnimationState(IDLE_ANIMATION_STATE);
                    }
                }
            }
        }

        private void Punch()
        {
            PlayAnimationState(FORWARD_SMASH_STATE, true);
        }
        
        private void StartWallHang()
        {
            PlayAnimationState(WALL_HIT_ANIMATION_STATE);
        }

        private void Dash(float _)
        {
            PlayAnimationState(FALL_ANIMATION_STATE);
        }
        
        private void Hook(Vector2 obj)
        {
            PlayAnimationState(FALL_ANIMATION_STATE);  
        }
        
        private void PlayAnimationState(string newState, bool canCallItself = false)
        {
            if (_currentAnimationState == newState && !canCallItself) return;
            
            _animator.Play(newState, 0, 0f);

            _currentAnimationState = newState;
        }
        
        private void Die()
        {
            UnsubscribeFromStateEvents();
            PlayAnimationState(IDLE_ANIMATION_STATE);
        }

        private void CompleteRespawn()
        {
            SubscribeToStateEvents();
        }
    }
}