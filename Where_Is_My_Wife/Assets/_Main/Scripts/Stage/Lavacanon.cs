using UnityEngine;
using WhereIsMyWife.Game;

namespace WhereIsMyWife.Lavacanon
{ 
    public class LavaCannon : PausableMonoBehaviour
    {
        [SerializeField] private GameObject _lavaProyectilPrefab; 
        [SerializeField] private Transform _firePoint;            
        [SerializeField] private float _minFireRate = 2f;         
        [SerializeField] private float _maxFireRate = 5f;         
        private float _nextFireTime = 0f;
        private float _timeSinceLastFire = 0f;
        public bool _shootLeft = true;

        protected override void Start()
        {
            base.Start();
            
            _nextFireTime = Time.time + Random.Range(_minFireRate, _maxFireRate);
        }

        protected override void OnUpdate()
        {
            if (_timeSinceLastFire >= _nextFireTime)
            {
                Shot();
                _nextFireTime = Random.Range(_minFireRate, _maxFireRate);
            }
            
            _timeSinceLastFire += Time.deltaTime;
        }

        void Shot()
        {
            _timeSinceLastFire = 0f;
            GameObject projectile = Instantiate(_lavaProyectilPrefab, _firePoint.position, _firePoint.rotation);
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            float direction = _shootLeft ? -1f : 1f;
            rb.velocity = _firePoint.right * direction * 5f;
        }
    }
}
