using UnityEngine;

namespace WhereIsMyWife.Managers.Properties
{
    public interface IPlayerCheckProperties
    {
        public Vector2 GroundCheckSize { get; }
        public Vector2 WallHangCheckSize { get; }
        public LayerMask GroundCheckLayerMask { get; }
        public LayerMask WallHangCheckLayerMask { get; }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerProperties/Modules/CheckProperties", 
        fileName = "CheckProperties")]
    public class PlayerCheckProperties : ScriptableObject, IPlayerCheckProperties
    {
        public IPlayerCheckProperties CheckProperties => this;
        
        [field:SerializeField] public Vector2 GroundCheckSize { get; private set; }
        [field:SerializeField] public Vector2 WallHangCheckSize { get; private set; }
        [field:SerializeField] public LayerMask GroundCheckLayerMask { get; private set; }
        [field:SerializeField] public LayerMask WallHangCheckLayerMask { get; private set; }
    }
}
