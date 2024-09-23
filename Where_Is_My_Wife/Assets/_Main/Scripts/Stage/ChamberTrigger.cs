using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

public class ChamberTrigger : MonoBehaviour
{
   IRespawn _playerRespawn;
   
   [SerializeField] private Transform _playerRespawnTransform;
   [SerializeField] private Transform _chamberTransform;

   private void Start()
   {
      _playerRespawn = PlayerManager.Instance.Respawn;
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("StageCollider"))
      {
         _playerRespawn.SetRespawnPoint(_playerRespawnTransform.position);
         PerformCameraAnimation();
      }
   }

   private void PerformCameraAnimation()
   {
      Vector3 nextPosition = _chamberTransform.position;
      nextPosition.z = -10;
         
      Camera.main.transform.DOMove(nextPosition, 0.5f);
   }
}
