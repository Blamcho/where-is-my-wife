using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

    public class LavaRise : MonoBehaviour
    {
         public float _riseSpeed = 1f;
        [SerializeField] private Transform _maxHeightTransform;
        [SerializeField] private Vector3 _initialPosition;
        [SerializeField] private bool _isRising = false;

        void Start()
        {
            _initialPosition = transform.position;

            PlayerManager.Instance.Respawn.RespawnStartAction += ResetLava;
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.Respawn.RespawnStartAction -= ResetLava;
        }

        void Update()
        {
            if (_isRising && transform.position.y < _maxHeightTransform.position.y)
            {
                transform.position += Vector3.up * _riseSpeed * Time.deltaTime;
            }
        }

        public void StartRising()
        {
            _isRising = true;
        }

        private void ResetLava(Vector3 _)
        {
            transform.position = _initialPosition;
            _isRising = false;
        }
    }


