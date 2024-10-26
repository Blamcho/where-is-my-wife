using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _playerRespawnTransform;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("StageCollider"))
            {
                PlayerManager.Instance.SetRespawnPoint(_playerRespawnTransform.position);
            }
        }
    }
}
