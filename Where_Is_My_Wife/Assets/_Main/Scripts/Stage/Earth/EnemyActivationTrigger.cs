using UnityEngine;
using WhereIsMyWife.Managers;

public class EnemyActivationTrigger : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;
    [SerializeField] private Transform _spawnTransform;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private bool _hasBeenTriggeredAlready;

    private void Start()
    {
        PlayerManager.Instance.RespawnStartAction += Reset;
    }

    private void Reset(Vector3 _)
    {
        _hasBeenTriggeredAlready = false;
        _particleSystem.Play();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_hasBeenTriggeredAlready)
        {
            _hasBeenTriggeredAlready = true;
            _enemyController.Activate(_spawnTransform.position);
            _particleSystem.Stop();
        }
    }
}
