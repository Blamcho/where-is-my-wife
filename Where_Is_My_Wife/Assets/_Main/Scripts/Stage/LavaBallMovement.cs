using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.LavaBalls
{
    public class LavaBallMovement : MonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _height = 3f;
        private Vector3 _startPos;
        private SpriteRenderer _spriteRenderer;
        private float _lastY;

        void Start()
        {
            _startPos = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _lastY = _startPos.y;
        }

        void Update()
        {
            float newY = Mathf.PingPong(Time.time * _speed, _height);
            transform.position = new Vector3(_startPos.x, _startPos.y + newY, _startPos.z);

            if (transform.position.y < _lastY)
            {
                _spriteRenderer.flipY = true;
            }
            else if (transform.position.y > _lastY)
            {
                _spriteRenderer.flipY = false;
            }

            _lastY = transform.position.y;
        }
    }
}
