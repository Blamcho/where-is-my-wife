using UnityEngine;

namespace WhereIsMyWife.Stage
{
    public class ScrollingBackground : MonoBehaviour
    {
        [SerializeField] private Transform _rightPartTransform;
        [SerializeField] private float _scrollingSpeed;
        [SerializeField] private float _cameraMovementMultiplier;
        
        private Camera _camera;
        private Vector3 _initialPosition;
        private Vector3 _lastCameraPosition;
        private Vector3 _currentCameraPosition;
        private float _backgroundWidth;
        
        private void Awake()
        {
            _initialPosition = transform.position;
            _camera = Camera.main;
            _lastCameraPosition = _camera.transform.position;
            _backgroundWidth = _rightPartTransform.localPosition.x;
        }

        private void Update()
        {
            if (HasReachedLimit())
            {
                transform.position = GetRecenterPosition();
            }
            
            transform.Translate(GetCameraMovementTranslation());
            transform.Translate(_scrollingSpeed * Time.deltaTime, 0, 0);
        }

        private Vector3 GetRecenterPosition()
        {
            Vector3 recenterPosition = transform.position;
            float centerOffset = Mathf.Abs(transform.position.x - _backgroundWidth);
            
            recenterPosition.x = _initialPosition.x + centerOffset * Mathf.Sign(_scrollingSpeed);

            return recenterPosition;
        }

        private Vector3 GetCameraMovementTranslation()
        {
            _currentCameraPosition = _camera.transform.position;
            Vector3 cameraPositionDifference = _currentCameraPosition - _lastCameraPosition;
            _lastCameraPosition = _currentCameraPosition;
            
            return cameraPositionDifference * (_cameraMovementMultiplier * -1);
        }

        private bool HasReachedLimit()
        {
            if (_scrollingSpeed < 0)
            {
                return transform.position.x < _initialPosition.x - _backgroundWidth;
            }
            
            return transform.position.x > _initialPosition.x + _backgroundWidth;
        }
    }
}
