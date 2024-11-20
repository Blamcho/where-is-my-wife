using UnityEngine;
using WhereIsMyWife.Game;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class MoveTowardsPlayer : PausableMonoBehaviour
    {
        [SerializeField] private float _velocity = 5f;
        [SerializeField] private float _lifeTime = 10f;
        float _timeSinceSpawned = 0f;

        protected override void Start()
        { 
            base.Start();
            
            FaceRightVectorTowardsPlayer();
            
            PlayerManager.Instance.RespawnStartAction += OnRespawn;
            
            if (BossManager.Instance != null)
            {
                BossManager.Instance.DieEvent += DestroyObject;
            }
        }

        protected override void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= OnRespawn;
            
            if (BossManager.Instance != null)
            {
                BossManager.Instance.DieEvent -= DestroyObject;
            }
            
            base.OnDestroy();
        }
        
        private void FaceRightVectorTowardsPlayer()
        {
            Vector2 direction = PlayerManager.Instance.PlayerControllerData.RigidbodyPosition - (Vector2)transform.position;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected override void OnUpdate()
        {
            transform.Translate(Vector3.right * (_velocity * Time.deltaTime), Space.Self);
            
            if (_timeSinceSpawned >= _lifeTime)
            {
                DestroyObject();
            }
        }

        private void OnRespawn(Vector3 _)
        {
            Destroy(gameObject);
        }
        
        private void DestroyObject()
        {
            Destroy(gameObject);
        }
    }
}
