using DG.Tweening;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
   [SerializeField] private Transform _chamberTransform;
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.CompareTag("StageCollider"))
      {
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
