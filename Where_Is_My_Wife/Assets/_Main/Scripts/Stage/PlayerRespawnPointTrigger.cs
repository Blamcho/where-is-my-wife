using UnityEngine;
using WhereIsMyWife.Managers;
using WhereIsMyWife.Controllers;

public class PlayerRespawnPointTrigger : MonoBehaviour
{
    IRespawn _playerRespawn;
   
    [SerializeField] private Transform _playerRespawnTransform;

    private void Start()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.Respawn != null)
        {
            _playerRespawn = PlayerManager.Instance.Respawn;
        }
        else
        {
            Debug.LogError("PlayerManager or Respawn is null");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StageCollider"))
        {
            _playerRespawn.SetRespawnPoint(_playerRespawnTransform.position);
        }
    }
}