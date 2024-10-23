using UnityEngine;
using WhereIsMyWife.Managers;

public class EnemyController : MonoBehaviour
{
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
}
