using UnityEngine;
using UnityEngine.Serialization;

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
            _rb = GetComponent<Rigidbody2D>();
            float direction = shootLeft ? -1f : 1f;
            _rb.velocity = transform.right * speed * direction;
            Destroy(gameObject, timelife);
        }
    }
}