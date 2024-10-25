using UnityEngine;

namespace WhereIsMyWife.UI
{
    public class Credits : MonoBehaviour
    {
        private Vector3 _initialPosition;
        public float _speed = 250f;
        private float _destroyY = 1080f;
        public RectTransform _rectTransform;
        private bool _isResetting = false;
        
        void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _initialPosition = _rectTransform.anchoredPosition;
        }

        void Update()
        {
            _rectTransform.anchoredPosition += Vector2.up * (_speed * Time.deltaTime);
        
            if (_rectTransform.anchoredPosition.y > _destroyY && !_isResetting)
            {
                _rectTransform.anchoredPosition = _initialPosition; 
            }
        }
    }
}

