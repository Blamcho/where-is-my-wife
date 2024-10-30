using Cinemachine;
using DG.Tweening;
using UnityEngine;

namespace WhereIsMyWife.Managers
{
    public class CameraManager : Singleton<CameraManager>
    {
        [SerializeField] private CinemachineVirtualCamera _currentCamera;

        public void MoveCameraToNextPosition(Vector3 nextPosition)
        {
            nextPosition.z = -10;
            _currentCamera.transform.DOMove(nextPosition, 0.5f);
        }

        public void ChangeCamera(CinemachineVirtualCamera nextCamera)
        {
            if (_currentCamera == nextCamera) return;
            
            nextCamera.enabled = true;
            
            _currentCamera.enabled = false;
            _currentCamera = nextCamera;
        }
    }
}
