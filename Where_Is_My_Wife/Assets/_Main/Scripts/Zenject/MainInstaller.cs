using WhereIsMyWife.Player.State;
using Zenject;

namespace WhereIsMyWife
{
    public class MainInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            InstallPlayerStates();
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
