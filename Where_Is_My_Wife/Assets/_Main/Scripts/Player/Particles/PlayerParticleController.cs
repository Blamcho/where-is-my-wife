using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Player.State;

namespace WhereIsMyWife.Controllers
{
    public class PlayerParticleController : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _runningParticles;
        [SerializeField] private ParticleSystem _jumpStartParticles;
        [SerializeField] private ParticleSystem _jumpingParticles;
        [SerializeField] private ParticleSystem _fallingParticles;
        [SerializeField] private ParticleSystem _dashingParticles;
        [SerializeField] private ParticleSystem _wallSlideParticles;
        [SerializeField] private ParticleSystem _wallJumpStartParticles;
        [SerializeField] private ParticleSystem _wallHangParticles;
        [SerializeField] private ParticleSystem _landParticles;
        [SerializeField] private ParticleSystem _hookParticles;
        [SerializeField] private ParticleSystem _deathParticles;
        
        private IPlayerStateIndicator _playerStateIndicator;
        private IPlayerStateInput _playerStateInput;
        private IRespawn _respawn;
        
        private IMovementStateEvents _movementStateEvents;
        private IWallHangStateEvents _wallHangStateEvents;
        private IDashStateEvents _dashStateEvents;
        private IHookStateEvents _hookStateEvents;

        private ParticleSystem _currentParticleSystem;
        
        private void Start()
        {
            _playerStateIndicator = PlayerManager.Instance.PlayerStateIndicator;
            _playerStateInput = PlayerManager.Instance.PlayerStateInput;
            _respawn = PlayerManager.Instance.Respawn;
            
            _movementStateEvents = PlayerManager.Instance.MovementStateEvents;
            _wallHangStateEvents = PlayerManager.Instance.WallHangStateEvents;
            _dashStateEvents = PlayerManager.Instance.DashStateEvents;
            _hookStateEvents = PlayerManager.Instance.HookStateEvents;
            
            SubscribeToStateEvents();
            SubscribeToRespawnEvents();
        }
        
        private void OnDestroy()
        {
            UnsubscribeFromStateEvents();
            UnsubscribeFromRespawnEvents();
        }

        private void PlayParticleSystem(ParticleSystem particleSystem, bool isOneShot = false)
        {
            if (_currentParticleSystem == particleSystem) return;
            
            if (!isOneShot)
            {
                _currentParticleSystem.Stop();
                _currentParticleSystem = particleSystem;
            }
            
            particleSystem.Play();
        }

        private void KillParticles()
        {
            _currentParticleSystem.Stop();
            _currentParticleSystem = null;
        }
        
        private void Run(float _)
        {
            if (!_playerStateIndicator.IsJumping)
            {
                if (_playerStateIndicator.IsRunFalling)
                {
                    PlayParticleSystem(_fallingParticles);
                }
                else if (_playerStateIndicator.IsAccelerating)
                {
                    PlayParticleSystem(_runningParticles);
                }
                else
                {
                    KillParticles();
                }
            }
            else
            {
                PlayParticleSystem(_jumpingParticles);
            }
        }
        
        private void Jump(float _)
        {
            if (_playerStateIndicator.IsOnWallHang)
            {
                PlayParticleSystem(_wallJumpStartParticles, true);
            }
            else
            {
                PlayParticleSystem(_jumpStartParticles, true);
            }
        }
        
        private void Fall(float _)
        {
            PlayParticleSystem(_fallingParticles);
        }
        
        private void Land()
        {
            PlayParticleSystem(_landParticles, true);
        }

        private void Dash(float _)
        {
            PlayParticleSystem(_dashingParticles);
        }
        
        private void StartWallHang()
        {
            PlayParticleSystem(_wallHangParticles, true);
            PlayParticleSystem(_wallSlideParticles);
        }

        private void Hook(Vector2 _)
        {
            PlayParticleSystem(_hookParticles);
        }
        
        private void Die()
        {
            UnsubscribeFromStateEvents();
            KillParticles();
            PlayParticleSystem(_deathParticles, true);
        }
        
        private void CompleteRespawn()
        {
            SubscribeToStateEvents();
        }
        
        private void SubscribeToStateEvents()
        {
            _movementStateEvents.JumpStart += Jump;
            _movementStateEvents.Run += Run;

            _wallHangStateEvents.StartWallHang += StartWallHang;
            _wallHangStateEvents.WallJumpStart += Fall;

            _dashStateEvents.Dash += Dash;

            _hookStateEvents.HookStart += Hook;
            
            _playerStateInput.Land += Land;
        }
        
        private void UnsubscribeFromStateEvents()
        {
            _movementStateEvents.JumpStart -= Jump;
            _movementStateEvents.Run -= Run;

            _wallHangStateEvents.StartWallHang -= StartWallHang;
            _wallHangStateEvents.WallJumpStart -= Fall;

            _dashStateEvents.Dash -= Dash;

            _hookStateEvents.HookStart -= Hook;
            
            _playerStateInput.Land -= Land;
        }
        
        private void SubscribeToRespawnEvents()
        {
            _respawn.DeathAction += Die;
            _respawn.RespawnCompleteAction += CompleteRespawn;
        }
        
        private void UnsubscribeFromRespawnEvents()
        {
            _respawn.DeathAction -= Die;
            _respawn.RespawnCompleteAction -= CompleteRespawn;
        }
    }
}
