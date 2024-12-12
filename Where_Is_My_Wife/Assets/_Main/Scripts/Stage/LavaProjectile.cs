using UnityEngine;
using WhereIsMyWife.Game;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Horizontal_Projectile_Lava
{
    public class LavaProjectile : PausableMonoBehaviour
    {
        [SerializeField] private float speed = 5f; 
        [SerializeField] private float timelife = 8f; 
        [SerializeField] private bool shootLeft = false;
        private Rigidbody2D _rb;
        private float _direction;
        private float _timeSinceSpawned = 0f;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        protected override void Start()
        {
            base.Start();
            
            PlayerManager.Instance.RespawnStartAction += DestroyProjectile;
            if (BossManager.Instance != null)
            {
                BossManager.Instance.StartFinalPhaseEvent += DestroyProjectile;
                BossManager.Instance.DieEvent += DestroyProjectile;
            }
            
            _direction = shootLeft ? -1f : 1f;
            SetCorrectVelocity();
        }
        

        protected override void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= DestroyProjectile;
            if (BossManager.Instance != null)
            {
                BossManager.Instance.StartFinalPhaseEvent -= DestroyProjectile;
                BossManager.Instance.DieEvent -= DestroyProjectile;
            }
            
            base.OnDestroy();
        }
        
        protected override void OnUpdate()
        {
            _timeSinceSpawned += Time.deltaTime;
            
            if (_timeSinceSpawned >= timelife)
            {
                DestroyProjectile();
            }
        }

        private void SetCorrectVelocity()
        {
            _rb.linearVelocity = transform.right * speed * _direction;
        }
        
        protected override void Pause()
        {
            _rb.linearVelocity = Vector3.zero;
        }

        protected override void Resume()
        {
            SetCorrectVelocity();
        }

        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }
        
        private void DestroyProjectile(Vector3 _)
        {
            Destroy(gameObject);
        }
    }
}