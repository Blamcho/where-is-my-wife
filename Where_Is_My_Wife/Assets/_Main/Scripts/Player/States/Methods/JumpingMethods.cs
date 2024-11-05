using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;

public interface IJumpingMethods
{
    public float GetJumpForce();
}

public class JumpingMethods : IJumpingMethods
{
    public IJumpingMethods Methods => this;
    
    private IPlayerStateIndicator _stateIndicator => PlayerManager.Instance.PlayerStateIndicator;
    
    private IPlayerJumpProperties _jumpProperties => PlayerManager.Instance.Properties.Jump;


    public float GetJumpForce()
    {
        return _jumpProperties.ForceMagnitude;
    }
}
