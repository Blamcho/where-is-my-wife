using UnityEngine;
using WhereIsMyWife.Managers;
using Cysharp.Threading.Tasks;

namespace WhereIsMyWife.Controllers
{
    public class RespawnTrigger : MonoBehaviour
    {
        [SerializeField] private Transform _playerRespawnTransform;
        [SerializeField] private GameObject _respawnPanel;
        [SerializeField] private Animator _respawnAnimator;
         [SerializeField] private IRespawn _playerRespawn;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("StageCollider"))
            {
                AnimateRespawnTransition().Forget();
            }
        }

        private async UniTaskVoid AnimateRespawnTransition()
        {
            _respawnAnimator.SetTrigger("StartTransition");
            _playerRespawn?.SetRespawnPoint(_playerRespawnTransform.position);
            
            await UniTask.Delay(1000);
            RespawnPlayer();
        }

        private void RespawnPlayer()
        {
            Debug.Log("Respawn complet.");
        }
    }
}
