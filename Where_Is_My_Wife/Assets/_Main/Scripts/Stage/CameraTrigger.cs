using Cinemachine;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
   public class CameraTrigger : MonoBehaviour
   {
      [SerializeField] private CameraAction _cameraAction;
   
      [Header("Move")]
      [SerializeField] private Transform _nextPosition;

      [Header("Change Camera")]
      [SerializeField] private CinemachineVirtualCamera _nextCamera;
   
      private enum CameraAction
      {
         Move,
         ChangeCamera,
      }
   
      private void OnTriggerEnter2D(Collider2D other)
      {
         if (other.CompareTag("StageCollider"))
         {
            switch (_cameraAction)
            {
               case CameraAction.Move:
                  CameraManager.Instance.MoveCameraToNextPosition(_nextPosition.position);
                  break;
               case CameraAction.ChangeCamera:
                  CameraManager.Instance.ChangeCamera(_nextCamera);
                  break;
            }
         }
      }
   }
}
