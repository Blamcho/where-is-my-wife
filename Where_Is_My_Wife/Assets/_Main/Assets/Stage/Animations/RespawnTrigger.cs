using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _playerRespawnTransform;
        
        private bool _hasBeenTriggered = false;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_hasBeenTriggered && other.CompareTag("StageCollider"))
            {
                _hasBeenTriggered = true;
                PlayerManager.Instance.SetRespawnPoint(_playerRespawnTransform.position);
            }
        }
    }
}
