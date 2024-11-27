using UnityEngine;
using WhereIsMyWife.Game;

namespace WhereIsMyWife.Stage
{
    public class ScrollingBackground : PausableMonoBehaviour
    {
        [SerializeField] private Transform _rightPartTransform;
        [SerializeField] private float _scrollingSpeed;
        [SerializeField] private float _cameraMovementMultiplier;
        
        private Camera _camera;
        private Vector3 _initialLocalPosition;
        private Vector3 _lastCameraPosition;
        private Vector3 _currentCameraPosition;
        private float _backgroundWidth;
        
        private void Awake()
        {
            _initialLocalPosition = transform.localPosition;
            _camera = Camera.main;
            _lastCameraPosition = _camera.transform.position;
            _backgroundWidth = _rightPartTransform.localPosition.x;
        }

        protected override void OnUpdate()
        {
            if (HasReachedLimit())
            {
                transform.localPosition = GetRecenterPosition();
            }
            
            transform.Translate(GetCameraMovementTranslation());
            transform.Translate(_scrollingSpeed * Time.deltaTime, 0, 0);
        }

        private Vector3 GetRecenterPosition()
        {
            Vector3 recenterPosition = transform.localPosition;
            float centerOffset = Mathf.Abs(transform.localPosition.x) - _backgroundWidth;
            
            recenterPosition.x = _initialLocalPosition.x + (centerOffset * Mathf.Sign(_scrollingSpeed));

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
                return transform.position.x < _initialLocalPosition.x - _backgroundWidth;
            }
            
            return transform.position.x > _initialLocalPosition.x + _backgroundWidth;
        }
    }
}
