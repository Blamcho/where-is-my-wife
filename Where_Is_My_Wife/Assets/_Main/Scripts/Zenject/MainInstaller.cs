using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;
using WhereIsMyWife.Player.State;
using WhereIsMyWife.Player.StateMachine;
using Zenject;

namespace WhereIsMyWife
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallActionMethods();
            InstallPlayerStates();
        }
        
        private void InstallActionMethods()
        {
            Container.BindInterfacesTo<RunningMethods>().AsSingle();
            Container.BindInterfacesTo<JumpingMethods>().AsSingle();
        }

        private void InstallPlayerStates()
        {
            Container.BindInterfacesTo<PlayerMovementState>().AsSingle();
            Container.BindInterfacesTo<PlayerDashState>().AsSingle();
            Container.BindInterfacesTo<PlayerWallHangState>().AsSingle();
            Container.BindInterfacesTo<PlayerWallJumpState>().AsSingle();
        }
    }
}
