using UnityEngine;
using WhereIsMyWife.Managers;
using Zenject;

namespace WhereIsMyWife.Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator _animator;

        private IMovementStateEvents _movementStateEvents;
        private IWallHangStateEvents _wallHangStateEvents;
        private IDashStateEvents _dashStateEvents;
        
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
            
            _movementStateEvents.JumpStart += _ => Jump();
            _movementStateEvents.Run+= _ => Run();

            _wallHangStateEvents.StartWallHang += StartWallHang;
            _wallHangStateEvents.WallJumpStart += _ => Fall();
        }

        private void OnDestroy()
        {
            _movementStateEvents.JumpStart -= _ => Jump();
            _movementStateEvents.Run -= _ => Run();

            _wallHangStateEvents.StartWallHang -= StartWallHang;
            _wallHangStateEvents.WallJumpStart -= _ => Fall();
        }

        private void Jump()
        {
            PlayAnimationState(JUMP_ANIMATION_STATE);
        }

        private void Fall()
        {
            PlayAnimationState(FALL_ANIMATION_STATE);
        }
        
        private void Run()
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