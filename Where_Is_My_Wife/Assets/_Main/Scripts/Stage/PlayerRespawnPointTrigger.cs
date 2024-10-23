using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;
using UnityEngine.UI;
using WhereIsMyWife.Controllers;


public class PlayerRespawnPointTrigger : MonoBehaviour
{
    IRespawn _playerRespawn;
   
    [SerializeField] private Transform _playerRespawnTransform;
    public GameObject _respawnPanel;  
    [SerializeField] private float _transitionDuration = 1f;  

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
        _respawnPanel.transform.localPosition = new Vector3(-Screen.width, 0, 0);
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

    private void AnimateRespawnTransition(TweenCallback onTransitionComplete)
    {
        _respawnPanel.transform.DOLocalMoveX(0, _transitionDuration)
            .OnComplete(() =>
            {
                _respawnPanel.transform.DOLocalMoveX(Screen.width, _transitionDuration)
                    .OnComplete(onTransitionComplete); 
            });
    }
}