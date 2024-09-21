using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Controllers;
using WhereIsMyWife.Managers;

public class ChamberTrigger : MonoBehaviour
{
   IRespawn _respawn;
   
   [SerializeField] private Transform _respawnTransform;
   [SerializeField] private Transform _chamberTransform;

   private void Start()
   {
      _respawn = PlayerManager.Instance.Respawn;
   }

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("StageCollider"))
      {
         _respawn.SetRespawnPoint(_respawnTransform.position);
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
