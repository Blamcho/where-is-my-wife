using UnityEngine;
using DG.Tweening;

namespace WhereIsMyWife.Controllers
{
    public class PlayerCameraFollowComponent : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransfom;

        private Vector3 _rotator = Vector3.zero;
        
        private void Update()
        {
            if (_playerTransfom.localScale.x < 0)
            {
                _rotator.y = 180;
            }
            else
            {
                _rotator.y = 0;
            }
            
            transform.DORotate(_rotator, 3f);
        }
    }
}
