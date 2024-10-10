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
        
        private IPlayerStateIndicator _playerStateIndicator;

        private Rigidbody2D _rigidbody2D;
        
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

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _playerStateIndicator = PlayerManager.Instance.PlayerStateIndicator;
            
            _movementStateEvents = PlayerManager.Instance.MovementStateEvents;
            _wallHangStateEvents = PlayerManager.Instance.WallHangStateEvents;
            _dashStateEvents = PlayerManager.Instance.DashStateEvents;
            _punchingStateEvents = PlayerManager.Instance.PunchingStateEvents;
            
            _movementStateEvents.JumpStart += Jump;
            _movementStateEvents.Run += Run;

            _wallHangStateEvents.StartWallHang += StartWallHang;
            _wallHangStateEvents.WallJumpStart += Fall;

            _punchingStateEvents.PunchStart += Punch;
        }

        private void OnDestroy()
        {
            _movementStateEvents.JumpStart -= Jump;
            _movementStateEvents.Run -= Run;

            _wallHangStateEvents.StartWallHang -= StartWallHang;
            _wallHangStateEvents.WallJumpStart -= Fall;
            
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
            PlayAnimationState(FORWARD_SMASH_STATE);
        }
        
        private void StartWallHang()
        {
            PlayAnimationState(WALL_HIT_ANIMATION_STATE);
        }
        
        private void PlayAnimationState(string newState)
        {
            if (_currentAnimationState == newState) return;
            
            _animator.Play(newState);

            _currentAnimationState = newState;
        }
    }
}