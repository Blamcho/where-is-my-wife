using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class MoveTowardsPlayer : MonoBehaviour
    {
        [SerializeField] private float _velocity = 5f;
    
        private void Start()
        { 
            FaceRightVectorTowardsPlayer();

            Destroy(gameObject, 10f);
        
            PlayerManager.Instance.RespawnStartAction += OnRespawn;
            
            if (BossManager.Instance != null)
            {
                BossManager.Instance.DieEvent += DestroyObject;
            }
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= OnRespawn;
            
            if (BossManager.Instance != null)
            {
                BossManager.Instance.DieEvent -= DestroyObject;
            }
        }

        private void FaceRightVectorTowardsPlayer()
        {
            Vector2 direction = PlayerManager.Instance.PlayerControllerData.RigidbodyPosition - (Vector2)transform.position;
        
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        private void Update()
        {
            transform.Translate(Vector3.right * (_velocity * Time.deltaTime), Space.Self);
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
