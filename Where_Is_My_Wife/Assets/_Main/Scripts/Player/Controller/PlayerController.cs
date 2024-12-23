using System;
using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Player.State;

namespace WhereIsMyWife.Controllers
{
    /// <summary>
    /// Reacts to whatever the <see cref="WhereIsMyWife.Player.StateMachine.PlayerStateMachine"/> decides the player needs to do based on the events sent.
    /// </summary>
    public class PlayerController : MonoBehaviour, IPlayerControllerData
    {
        public Vector2 RigidbodyVelocity => _rigidbody2D.velocity;
        public Vector2 RigidbodyPosition => _rigidbody2D.position;
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
        private IPunchingStateEvents _punchingStateEvents;

        private IPlayerStateIndicator _playerStateIndicator;
        private IPlayerStateInput _playerStateInput;
        private IPlayerControllerEvent _playerControllerEvent;
        private IRespawn _respawn;

        [SerializeField]
        private Rigidbody2D _rigidbody2D;

        [SerializeField]
        private Transform _groundCheckTransform = null;

        [SerializeField]
        private Transform _wallHangCheckUpTransform = null;

        [SerializeField]
        private Transform _wallHangCheckDownTransform = null;

        [SerializeField]
        private SpriteRenderer _spriteRenderer = null;

        [SerializeField, Range(0, 1)]
        private float _dashColorValue = 0.4f;

        [SerializeField]
        private Ease _dashColorEase = Ease.OutSine;

        [SerializeField]
        private float _dashColorDuration = 0.5f;

        private Tween _dashColorTween;
        private Vector2 _velocityBeforePause;
        private float _gravityScaleBeforePause;
        
        private bool _isDead = false;

        private void Start()
        {
            _movementStateEvents = PlayerManager.Instance.MovementStateEvents;
            _wallHangStateEvents = PlayerManager.Instance.WallHangStateEvents;
            _wallJumpStateEvents = PlayerManager.Instance.WallJumpStateEvents;
            _dashStateEvents = PlayerManager.Instance.DashStateEvents;
            _hookStateEvents = PlayerManager.Instance.HookStateEvents;
            _punchingStateEvents = PlayerManager.Instance.PunchingStateEvents;

            _playerStateIndicator = PlayerManager.Instance.PlayerStateIndicator;
            _playerStateInput = PlayerManager.Instance.PlayerStateInput;
            _playerControllerEvent = PlayerManager.Instance.PlayerControllerEvent;
            _respawn = PlayerManager.Instance.Respawn;

            _playerControllerEvent.SetPlayerControllerData(this);

            SubscribeToStateEvents();
            SubscribeToRespawnEvents();

            GameManager.Instance.PauseEvent += Pause;
            GameManager.Instance.ResumeEvent += Resume;
        }

        private void OnDestroy()
        {
            UnsubscribeFromStateEvents();
            UnsubscribeFromRespawnEvents();

            GameManager.Instance.PauseEvent -= Pause;
            GameManager.Instance.ResumeEvent -= Resume;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            TriggerEnterEvent?.Invoke(collision);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            TriggerExitEvent?.Invoke(collision);
        }

        private void Pause()
        {
            if (!_isDead) UnsubscribeFromStateEvents();

            _velocityBeforePause = _rigidbody2D.velocity;
            _gravityScaleBeforePause = _rigidbody2D.gravityScale;
            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Resume()
        {
            if (!_isDead) SubscribeToStateEvents();

            _rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rigidbody2D.velocity = _velocityBeforePause;
            _rigidbody2D.gravityScale = _gravityScaleBeforePause;
        }

        private void SimpleJump(float jumpForce)
        {
            AudioManager.Instance.PlaySFX("Jump");
            JumpStart(jumpForce);
        }

        private void WallJump(float jumpForce)
        {
            AudioManager.Instance.PlaySFX("Jump");
            JumpStart(jumpForce);
        }

        private void DoubleJump(float jumpForce)
        {
            AudioManager.Instance.PlaySFX("DoubleJump");
            JumpStart(jumpForce);
        }

        private void JumpStart(float jumpForce)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
            _rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        private void RunAndFaceDirection(float runAcceleration)
        {
            FaceDirection(_playerStateIndicator.IsRunningRight);
            Run(runAcceleration);
        }

        private void Run(float runAcceleration)
        {
            _rigidbody2D.AddForce(Vector2.right * runAcceleration, ForceMode2D.Force);
        }

        private void SetPosition(Vector2 position)
        {
            _rigidbody2D.position = position;
        }

        private void HookStart(Vector2 velocity)
        {
            AudioManager.Instance.PlaySFX("Hook");
            FaceDirection(velocity.x > 0);
            AddImpulse(velocity);
        }

        private void AddImpulse(Vector2 velocity)
        {
            _rigidbody2D.velocity = Vector2.zero;
            _rigidbody2D.AddForce(velocity, ForceMode2D.Impulse);
        }

        private void SetGravityScale(float gravityScale)
        {
            _rigidbody2D.gravityScale = gravityScale;
        }

        private void SetFallSpeedCap(float fallSpeedCap)
        {
            _rigidbody2D.velocity = new Vector2(
                _rigidbody2D.velocity.x,
                Mathf.Max(_rigidbody2D.velocity.y, -fallSpeedCap)
            );
        }

        private void SetFallSpeed(float fallSpeed)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, -fallSpeed);
        }

        private void Dash(float speed)
        {
            AudioManager.Instance.PlaySFX("Dash");
            _dashColorTween?.Kill();
            _spriteRenderer.color = Color.HSVToRGB(0, 0, _dashColorValue);

            FaceDirection(speed > 0);
            SetHorizontalSpeed(speed);
        }

        private void Land()
        {
            AudioManager.Instance.PlaySFX("Landing");
            _dashColorTween = _spriteRenderer
                .DOColor(Color.white, _dashColorDuration)
                .SetEase(_dashColorEase);
        }

        private void SetHorizontalSpeed(float speed)
        {
            _rigidbody2D.velocity = new Vector2(speed, _rigidbody2D.velocity.y);
        }

        private void WallHangVelocity(float fallVelocity)
        {
            SetGravityScale(0f);
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, fallVelocity);
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

        private void Die()
        {
            _isDead = true;
            gameObject.SetActive(false);
            UnsubscribeFromStateEvents();
        }

        private void StartRespawn(Vector3 respawnPosition)
        {
            _spriteRenderer.color = Color.white;
            gameObject.SetActive(true);
            transform.position = respawnPosition;
        }

        private void CompleteRespawn()
        {
            _isDead = false;
            SubscribeToStateEvents();
        }

        private void Punch()
        {
            AudioManager.Instance.PlaySFX("Punching");
        }

        private void StartWallHang()
        {
            AudioManager.Instance.PlaySFX("WallHang");
        }

        private void SubscribeToStateEvents()
        {
            _movementStateEvents.Run += RunAndFaceDirection;
            _movementStateEvents.JumpStart += SimpleJump;
            _movementStateEvents.GravityScale += SetGravityScale;
            _movementStateEvents.FallSpeedCap += SetFallSpeedCap;

            _wallHangStateEvents.WallHangVelocity += WallHangVelocity;
            _wallHangStateEvents.Turn += Turn;
            _wallHangStateEvents.WallJumpStart += WallJump;
            _wallHangStateEvents.StartWallHang += StartWallHang;

            _wallJumpStateEvents.WallJumpVelocity += SetHorizontalSpeed;
            _wallJumpStateEvents.GravityScale += SetGravityScale;
            _wallJumpStateEvents.FallSpeedCap += SetFallSpeedCap;
            _wallJumpStateEvents.DoubleJump += DoubleJump;

            _dashStateEvents.DashStart += Dash;
            _dashStateEvents.GravityScale += SetGravityScale;
            _dashStateEvents.FallSpeedCap += SetFallSpeedCap;
            _dashStateEvents.FallingSpeed += SetFallSpeed;

            _hookStateEvents.GravityScale += SetGravityScale;
            _hookStateEvents.HookStart += HookStart;
            _hookStateEvents.SetPosition += SetPosition;

            _punchingStateEvents.Run += Run;
            _punchingStateEvents.PunchStart += Punch;
            _punchingStateEvents.JumpStart += SimpleJump;
            _punchingStateEvents.GravityScale += SetGravityScale;
            _punchingStateEvents.FallSpeedCap += SetFallSpeedCap;

            _playerStateInput.Land += Land;
        }

        private void UnsubscribeFromStateEvents()
        {
            _movementStateEvents.Run -= RunAndFaceDirection;
            _movementStateEvents.JumpStart -= SimpleJump;
            _movementStateEvents.GravityScale -= SetGravityScale;
            _movementStateEvents.FallSpeedCap -= SetFallSpeedCap;

            _wallHangStateEvents.WallHangVelocity -= WallHangVelocity;
            _wallHangStateEvents.Turn -= Turn;
            _wallHangStateEvents.WallJumpStart -= WallJump;
            _wallHangStateEvents.StartWallHang -= StartWallHang;

            _wallJumpStateEvents.WallJumpVelocity -= SetHorizontalSpeed;
            _wallJumpStateEvents.GravityScale -= SetGravityScale;
            _wallJumpStateEvents.FallSpeedCap -= SetFallSpeedCap;
            _wallJumpStateEvents.DoubleJump -= DoubleJump;

            _dashStateEvents.DashStart -= Dash;
            _dashStateEvents.GravityScale -= SetGravityScale;
            _dashStateEvents.FallSpeedCap -= SetFallSpeedCap;
            _dashStateEvents.FallingSpeed -= SetFallSpeed;

            _hookStateEvents.GravityScale -= SetGravityScale;
            _hookStateEvents.HookStart -= HookStart;
            _hookStateEvents.SetPosition -= SetPosition;

            _punchingStateEvents.Run -= Run;
            _punchingStateEvents.PunchStart -= Punch;
            _punchingStateEvents.JumpStart -= SimpleJump;
            _punchingStateEvents.GravityScale -= SetGravityScale;
            _punchingStateEvents.FallSpeedCap -= SetFallSpeedCap;

            _playerStateInput.Land -= Land;
        }

        private void SubscribeToRespawnEvents()
        {
            _respawn.DeathAction += Die;
            _respawn.RespawnStartAction += StartRespawn;
            _respawn.RespawnCompleteAction += CompleteRespawn;
        }

        private void UnsubscribeFromRespawnEvents()
        {
            _respawn.DeathAction -= Die;
            _respawn.RespawnStartAction -= StartRespawn;
            _respawn.RespawnCompleteAction -= CompleteRespawn;
        }
    }
}
