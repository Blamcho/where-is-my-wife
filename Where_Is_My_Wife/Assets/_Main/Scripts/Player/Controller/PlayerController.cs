using System;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
    /// <summary>
    /// Reacts to whatever the <see cref="WhereIsMyWife.Player.StateMachine.PlayerStateMachine"/> decides the player needs to do based on the events sent. 
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerControllerData
    {
        public Vector2 RigidbodyVelocity => _rigidbody2D.velocity;
        public Transform RigidbodyTransform => _rigidbody2D.transform;
        public Vector2 GroundCheckPosition => _groundCheckTransform.position;
        public Vector2 WallHangCheckUpPosition => _wallHangCheckUpTransform.position;
        public Vector2 WallHangCheckDownPosition => _wallHangCheckDownTransform.position;
        public float HorizontalScale => transform.localScale.x;
        public Action<Collider2D> TriggerEnterEvent { get; set; }
        public Action<Collider2D> TriggerExitEvent { get; set; }

        private IMovementStateEvents _movementStateEvents;
        private IWallHangStateEvents _wallHangStateEvents;
        private IWallJumpStateEvents _wallJumpStateEvents;
        private IDashStateEvents _dashStateEvents;
        private IHookStateEvents _hookStateEvents;
        
        private IPlayerStateIndicator _playerStateIndicator;
        private IPlayerControllerEvent _playerControllerEvent;
        private IRespawn _respawn;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private Transform _groundCheckTransform = null;
        [SerializeField] private Transform _wallHangCheckUpTransform = null;
        [SerializeField] private Transform _wallHangCheckDownTransform = null;
        
        private void Start()
        {
            _movementStateEvents = PlayerManager.Instance.MovementStateEvents;
            _wallHangStateEvents = PlayerManager.Instance.WallHangStateEvents;
            _wallJumpStateEvents = PlayerManager.Instance.WallJumpStateEvents;
            _dashStateEvents = PlayerManager.Instance.DashStateEvents;
            _hookStateEvents = PlayerManager.Instance.HookStateEvents;

            _playerStateIndicator = PlayerManager.Instance.PlayerStateIndicator;
            _playerControllerEvent = PlayerManager.Instance.PlayerControllerEvent;
            _respawn = PlayerManager.Instance.Respawn;
            
            _playerControllerEvent.SetPlayerControllerData(this);
            
            SubscribeToObservables();
        }

        private void OnDestroy()
        {
            UnsubscribeToObservables();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerEnterEvent?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            TriggerExitEvent?.Invoke(collision);
        }

        private void SubscribeToObservables()
        {
            _movementStateEvents.Run += Run; 
            _movementStateEvents.JumpStart += JumpStart;
            _movementStateEvents.GravityScale += SetGravityScale;
            _movementStateEvents.FallSpeedCap += SetFallSpeedCap;

            _wallHangStateEvents.WallHangVelocity += WallHangVelocity;
            _wallHangStateEvents.Turn += Turn;
            _wallHangStateEvents.WallJumpStart += JumpStart;

            _wallJumpStateEvents.WallJumpVelocity += SetHorizontalSpeed;
            _wallJumpStateEvents.GravityScale += SetGravityScale;
            _wallJumpStateEvents.FallSpeedCap += SetFallSpeedCap;

            _dashStateEvents.Dash += SetHorizontalSpeed;
            _dashStateEvents.GravityScale += SetGravityScale;
            _dashStateEvents.FallSpeedCap += SetFallSpeedCap;
            _dashStateEvents.FallingSpeed += SetFallSpeed;
            
            _hookStateEvents.StartHook += StartHook;
            _hookStateEvents.ExecuteHook += ExecuteHookLaunch;
            _hookStateEvents.HookQTEFailed += ResumeVelocityAfterHookQTEFailed;
            
            _respawn.RespawnAction += Respawn;
        }

        private void UnsubscribeToObservables()
        {
            _movementStateEvents.Run -= Run; 
            _movementStateEvents.JumpStart -= JumpStart;
            _movementStateEvents.GravityScale -= SetGravityScale;
            _movementStateEvents.FallSpeedCap -= SetFallSpeedCap;

            _wallHangStateEvents.WallHangVelocity -= WallHangVelocity;
            _wallHangStateEvents.Turn -= Turn;
            _wallHangStateEvents.WallJumpStart -= JumpStart;

            _wallJumpStateEvents.WallJumpVelocity -= SetHorizontalSpeed;
            _wallJumpStateEvents.GravityScale -= SetGravityScale;
            _wallJumpStateEvents.FallSpeedCap -= SetFallSpeedCap;

            _dashStateEvents.Dash -= SetHorizontalSpeed;
            _dashStateEvents.GravityScale -= SetGravityScale;
            _dashStateEvents.FallSpeedCap -= SetFallSpeedCap;
            _dashStateEvents.FallingSpeed -= SetFallSpeed;
            
            _hookStateEvents.StartHook -= StartHook;
            _hookStateEvents.ExecuteHook -= ExecuteHookLaunch;
            _hookStateEvents.HookQTEFailed -= ResumeVelocityAfterHookQTEFailed;

            _respawn.RespawnAction -= Respawn;
        }

        private void JumpStart(float jumpForce)
        {
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void Run(float runAcceleration)
        {
            FaceDirection(_playerStateIndicator.IsRunningRight);
            _rigidbody2D.AddForce(Vector2.right * runAcceleration, ForceMode2D.Force);
        }

        private void SetGravityScale(float gravityScale)
        {
            _rigidbody2D.gravityScale = gravityScale;
        }

        private void SetFallSpeedCap(float fallSpeedCap)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                Mathf.Max(_rigidbody2D.velocity.y, -fallSpeedCap));
        }

        private void SetFallSpeed(float fallSpeed)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -fallSpeed);
        }


        private void SetHorizontalSpeed(float speed)
        {
            _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);
        }
        
        private void WallHangVelocity(float fallVelocity)
        {
            SetGravityScale(0f);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x,
                fallVelocity);
        }

        private void Turn()
        {
            Vector3 scale = transform.localScale;

            scale.x *= -1;
            
            transform.localScale = scale;
        }
        
        private void FaceDirection(bool shouldFaceRight)
        {
            Vector3 scale = transform.localScale;
            
            if (shouldFaceRight)
            {
                scale.x = 1;
            }

            else
            {
                scale.x = -1;
            }

            transform.localScale = scale;
        }
        
        private void Respawn(Vector3 respawnPosition)
        {
            transform.position = respawnPosition;
        }

        private void StartHook()
        {
            _rigidbody2D.velocity = Vector2.zero;
        }

        private void ExecuteHookLaunch(Vector2 hookVelocity)
        {
            _rigidbody2D.AddForce(hookVelocity, ForceMode2D.Impulse);
        }

        private void ResumeVelocityAfterHookQTEFailed(Vector2 originalPlayerVelocity)
        {
            _rigidbody2D.velocity = originalPlayerVelocity;
        }
    }
}
