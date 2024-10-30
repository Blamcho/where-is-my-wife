using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WhereIsMyWife.Stage
{
    [RequireComponent(typeof(Collider2D))]
    public class OneWayWall : MonoBehaviour
    {
        [SerializeField] private bool _wallIsOnRightSide;
        [SerializeField] private int _targetLayer;
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private float _fadeInDuration = 0.15f;
        
        private Collider2D _collider2D;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("Exiting");
            if (!other.CompareTag("Player")) return;
            
            bool playerExitedFromTheRight = other.transform.position.x > transform.position.x;
            
            if (playerExitedFromTheRight == _wallIsOnRightSide)
            {
                _collider2D.isTrigger = false;
                gameObject.layer = _targetLayer;
                Color currentColor = _tilemap.color;
                DOTween.To(() => _tilemap.color, x => _tilemap.color = x, Color.white, _fadeInDuration);
            }
        }
    }
}
