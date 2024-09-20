using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace WhereIsMyWife.Managers.Properties
{
    public interface IPlayerProperties
    {
        public IPlayerMovementProperties Movement { get; }
        public IPlayerJumpProperties Jump { get; }
        public IPlayerDashProperties Dash { get; }
        public IPlayerGravityProperties Gravity { get; }
        public IPlayerCheckProperties Check { get; }
        public IPlayerWallJumpProperties WallJump { get; }
    }
    
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerProperties/PlayerProperties", fileName = "PlayerProperties")]
    public class PlayerProperties : ScriptableObject, IPlayerProperties
    {
        public IPlayerProperties Properties => this;
        
        public IPlayerMovementProperties Movement => _movementPropertiesSO.MovementProperties;
        public IPlayerJumpProperties Jump => _jumpPropertiesSO.JumpProperties;
        public IPlayerDashProperties Dash => _dashPropertiesSO.DashProperties;
        public IPlayerGravityProperties Gravity => _gravityPropertiesSO.GravityProperties;
        public IPlayerCheckProperties Check => _checkPropertiesSO.CheckProperties;
        public IPlayerWallJumpProperties WallJump => _wallJumpPropertiesSO.WallJumpProperties;

        [field: SerializeField] private PlayerMovementProperties _movementPropertiesSO;
        [field: SerializeField] private PlayerJumpProperties _jumpPropertiesSO;
        [field: SerializeField] private PlayerDashProperties _dashPropertiesSO;
        [field: SerializeField] private PlayerGravityProperties _gravityPropertiesSO;
        [field: SerializeField] private PlayerCheckProperties _checkPropertiesSO;
        [field: SerializeField] private PlayerWallJumpProperties _wallJumpPropertiesSO;
    }
}
