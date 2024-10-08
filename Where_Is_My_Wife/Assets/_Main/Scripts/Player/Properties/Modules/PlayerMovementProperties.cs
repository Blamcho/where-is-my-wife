using UnityEngine;

namespace WhereIsMyWife.Managers.Properties
{
    public interface IPlayerMovementProperties
    {
        public float RunMaxSpeed { get; }
        public float RunAccelerationRate { get; }
        public float RunDecelerationRate { get; }
        public float AirAccelerationMultiplier { get; }
        public float AirDecelerationMultiplier { get; }
        public float WallSlideMaxVelocity { get; }
        public float WallSlideTimeToMaxVelocity { get; }
        public float WallSlideFastVelocity { get; }
        public float WallHangCancelBuffer { get; }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerProperties/Modules/MovementProperties", fileName = "MovementProperties")]
    public class PlayerMovementProperties : ScriptableObject, IPlayerMovementProperties
    {
        public IPlayerMovementProperties MovementProperties => this;
        
        [field: SerializeField] public float RunMaxSpeed { get; private set; }
        [field: SerializeField] public float RunAccelerationRate { get; private set; }
        [field: SerializeField] public float RunDecelerationRate { get; private set; }
        [field: SerializeField] public float AirAccelerationMultiplier { get; private set; }
        [field: SerializeField] public float AirDecelerationMultiplier { get; private set; }
        [field: SerializeField] public float WallSlideMaxVelocity { get; private set; }
        [field: SerializeField] public float WallSlideTimeToMaxVelocity { get; private set; }
        [field: SerializeField] public float WallSlideFastVelocity { get; private set; }
        [field: SerializeField] public float WallHangCancelBuffer { get; private set; }
    }

}