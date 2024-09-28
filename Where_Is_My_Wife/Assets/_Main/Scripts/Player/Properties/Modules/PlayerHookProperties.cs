using UnityEngine;

namespace WhereIsMyWife.Managers.Properties
{
    public interface IPlayerHookProperties
    {
        public float ThrustForce { get; }
        public float TimeToReachHookPosition { get; }
    }

    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerProperties/Modules/HookProperties", fileName = "HookProperties")]
    public class PlayerHookProperties : ScriptableObject, IPlayerHookProperties
    {
        public IPlayerHookProperties HookProperties => this;

        [field: SerializeField] public float ThrustForce { get; private set;}
        [field: SerializeField] public float TimeToReachHookPosition { get; private set; }
    }

}

