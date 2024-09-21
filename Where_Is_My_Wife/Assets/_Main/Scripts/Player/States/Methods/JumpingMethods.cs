using WhereIsMyWife.Managers;
using WhereIsMyWife.Managers.Properties;

public interface IJumpingMethods
{
    public float GetJumpForce(float currentVelocityY);
}

public class JumpingMethods : IJumpingMethods
{
    public IJumpingMethods Methods => this;
    
    private IPlayerStateIndicator _stateIndicator => PlayerManager.Instance.PlayerStateIndicator;
    
    private IPlayerJumpProperties _jumpProperties => PlayerManager.Instance.Properties.Jump;


    public float GetJumpForce(float currentVelocityY)
    {
        float force = _jumpProperties.ForceMagnitude;

        if (currentVelocityY < 0)
        {
            force -= currentVelocityY; // To always jump the same amount.
        }

        return force;
    }
}
