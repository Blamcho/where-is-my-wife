using UnityEngine;
using WhereIsMyWife.Managers;
using UnityEngine.UI;
using WhereIsMyWife.Controllers;

public class RespawPointTrasition : MonoBehaviour
{
    IRespawn _playerRespawn;
   
    [SerializeField] private Transform _playerRespawnTransform;
    [SerializeField] private GameObject _respawnPanel;  
     public Animator _respawnAnimator;
     
    private void Start()
    {
        if (PlayerManager.Instance != null && PlayerManager.Instance.Respawn != null)
        {
            _playerRespawn = PlayerManager.Instance.Respawn;
        }
        else
        {
            Debug.LogError("PlayerManager o Respawn es null");
        }

        if (_respawnAnimator == null)
        {
            Debug.LogError("Animator no asignado en el Inspector");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("StageCollider"))
        {
         
            AnimateRespawnTransition(() => {
                _playerRespawn.SetRespawnPoint(_playerRespawnTransform.position);
            });
        }
    }

    private void AnimateRespawnTransition(System.Action onTransitionComplete)
    {
        _respawnAnimator.SetTrigger("StartTransition");
        
        Invoke(nameof(RespawnComplete), 1f); 
        onTransitionComplete?.Invoke();
    }

    private void RespawnComplete()
    {
        Debug.Log("Respawn completado.");
    }
}