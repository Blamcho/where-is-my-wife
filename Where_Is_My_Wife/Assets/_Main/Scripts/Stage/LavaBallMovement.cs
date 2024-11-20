using UnityEngine;
using WhereIsMyWife.Game;

namespace WhereIsMyWife.LavaBalls
{
    public class LavaBallMovement : PausableMonoBehaviour
    {
        [SerializeField] private float _speed = 2f;
        [SerializeField] private float _height = 3f;
        private Vector3 _startPos;
        private SpriteRenderer _spriteRenderer;
        private float _lastY;
        private float _timeSinceSpawn = 0f;

        protected override void Start()
        {
            base.Start();
            _startPos = transform.position;
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _lastY = _startPos.y;
        }

        protected override void OnUpdate()
        {
            _timeSinceSpawn += Time.deltaTime;
            
            float newY = Mathf.PingPong(_timeSinceSpawn * _speed, _height);
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
