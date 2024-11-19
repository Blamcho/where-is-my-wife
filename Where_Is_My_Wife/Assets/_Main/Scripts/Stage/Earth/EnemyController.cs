using UnityEngine;
using WhereIsMyWife.Managers;

public class EnemyController : MonoBehaviour, IPunchable
{
    [SerializeField] protected ParticleSystem _deathParticlesPrefab;
    [SerializeField] protected bool _isBossEnemy = false;
    
    protected Vector2 _playerPosition;
    protected bool _isMovingTowardsPlayer;
    
    private void Start()
    {
        PlayerManager.Instance.RespawnStartAction += Deactivate;
            
        gameObject.SetActive(false);    
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.RespawnStartAction += Deactivate;
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
    
    protected void Update()
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
