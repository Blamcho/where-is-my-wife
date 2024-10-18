using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _currentCamera;

    public void MoveCameraToNextPosition(Vector3 nextPosition)
    {
        nextPosition.z = -10;
        _currentCamera.transform.DOMove(nextPosition, 0.5f);
    }
}
