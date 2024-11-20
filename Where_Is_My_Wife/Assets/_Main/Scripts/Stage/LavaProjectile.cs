using System;
using UnityEngine;
using UnityEngine.Serialization;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Horizontal_Projectile_Lava
{
    public class LavaProjectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f; 
        [SerializeField] private float timelife = 8f; 
        [SerializeField] private bool shootLeft = false;
        private Rigidbody2D _rb;

        void Start()
        {
            PlayerManager.Instance.RespawnStartAction += DestroyProjectile;
            if (BossManager.Instance != null) BossManager.Instance.StartFinalPhaseEvent += DestroyProjectile;
            
            _rb = GetComponent<Rigidbody2D>();
            float direction = shootLeft ? -1f : 1f;
            _rb.velocity = transform.right * speed * direction;
            Destroy(gameObject, timelife);
        }
        
        private void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= DestroyProjectile;
            if (BossManager.Instance != null) BossManager.Instance.StartFinalPhaseEvent -= DestroyProjectile;
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