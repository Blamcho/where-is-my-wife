using UnityEngine;

namespace WhereIsMyWife.Managers.Properties
{
    public interface IPlayerWallJumpProperties
    {
        public float MinWallJumpDuration { get; }
        public float Speed { get; }
        public float TimeToNormalSpeed { get; }
        public float TimeToZeroSpeed { get; }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerProperties/Modules/WallJumpProperties", fileName = "WallJumpProperties")]
    public class PlayerWallJumpProperties : ScriptableObject, IPlayerWallJumpProperties
    {
        public IPlayerWallJumpProperties WallJumpProperties => this;
        
        [field: SerializeField] public float MinWallJumpDuration { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float TimeToNormalSpeed { get; private set; }
        [field: SerializeField] public float TimeToZeroSpeed { get; private set; }
    }

}