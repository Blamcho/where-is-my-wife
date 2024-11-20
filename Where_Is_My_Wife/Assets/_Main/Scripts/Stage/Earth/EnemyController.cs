using UnityEngine;
using WhereIsMyWife.Game;
using WhereIsMyWife.Managers;

public class EnemyController : PausableMonoBehaviour, IPunchable
{
    [SerializeField] protected ParticleSystem _deathParticlesPrefab;
    [SerializeField] protected bool _isBossEnemy = false;
    
    protected Vector2 _playerPosition;
    protected bool _isMovingTowardsPlayer;

    protected override void Start()
    {
        base.Start();
        
        PlayerManager.Instance.RespawnStartAction += Deactivate;
            
        gameObject.SetActive(false);    
    }

    protected override void OnDestroy()
    {
        PlayerManager.Instance.RespawnStartAction += Deactivate;
        
        base.OnDestroy();
    }
    
    public virtual void Activate(Vector2 position)
    {
        gameObject.SetActive(true);
        transform.position = position;
    }
    
    public virtual void Deactivate()
    {
        gameObject.SetActive(false);
    }
    
    private void Deactivate(Vector3 _)
    {
        Deactivate();
    }
    
    protected override void OnUpdate()
    {
        if (_isMovingTowardsPlayer)
        {
            UpdatePlayerPosition();
        }
    }

    protected void UpdatePlayerPosition()
    {
        _playerPosition = PlayerManager.Instance.PlayerControllerData.RigidbodyPosition;
    }

    public void Punch()
    {
        if (_isBossEnemy)
        {
            BossManager.Instance.TakeDamage();
        }

        Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
        Deactivate();
    }
}
