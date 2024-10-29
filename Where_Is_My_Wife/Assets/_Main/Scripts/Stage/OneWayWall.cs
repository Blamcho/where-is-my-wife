using UnityEngine;

namespace WhereIsMyWife.Stage
{
    [RequireComponent(typeof(Collider2D))]
    public class OneWayWall : MonoBehaviour
    {
        [SerializeField] private bool _wallIsOnRightSide;
        [SerializeField] private int _targetLayer;
        
        private Collider2D _collider2D;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            bool playerExitedFromTheRight = other.transform.position.x > transform.position.x;
            
            if (playerExitedFromTheRight == _wallIsOnRightSide)
            {
                _collider2D.isTrigger = false;
                gameObject.layer = _targetLayer;
            }
        }
    }
}
