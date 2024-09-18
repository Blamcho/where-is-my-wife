using UnityEngine;
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
        [Inject] public IPlayerMovementProperties Movement { get; private set; }
        [Inject] public IPlayerJumpProperties Jump { get; private set; }
        [Inject] public IPlayerDashProperties Dash { get; private set; }
        [Inject] public IPlayerGravityProperties Gravity { get; private set; }
        [Inject] public IPlayerCheckProperties Check { get; private set; }
        [Inject] public IPlayerWallJumpProperties WallJump { get; private set; }
        
        [field:SerializeField] public PlayerMovementProperties MovementProperties { get; private set; }
        [field:SerializeField] public PlayerJumpProperties JumpProperties { get; private set; }
        [field:SerializeField] public PlayerDashProperties DashProperties { get; private set; }
        [field:SerializeField] public PlayerGravityProperties GravityProperties { get; private set; }
        [field:SerializeField] public PlayerCheckProperties CheckProperties { get; private set; }
        [field:SerializeField] public PlayerWallJumpProperties WallJumpProperties { get; private set; }
    }
}