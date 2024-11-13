using UnityEngine;
using DG.Tweening;

namespace WhereIsMyWife.Controllers
{
    public class PlayerCameraFollowComponent : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;

        private int _lastRotation;
        
        private void Update()
        {
            var currentRotation = _playerTransform.localScale.x < 0 ? 180 : 0;

            if (_lastRotation == currentRotation) return;

            _lastRotation = currentRotation;
            transform.DOKill();
            transform.DORotate(new Vector3(0, _lastRotation, 0), 3f);
        }
    }
}
