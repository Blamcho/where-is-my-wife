using DG.Tweening;
using UnityEngine;

namespace WhereIsMyWife.Stage
{
    public class DoubleJumpTrigger : MonoBehaviour
    {
        [SerializeField] private Color _activeColor = Color.green;
        [SerializeField] private float _respawnDelay = 5f;
        [SerializeField] private float _respawnFadeDuration = 0.5f;
        
        private SpriteRenderer _spriteRenderer;
        private Tween _respawnTween;
        
        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _spriteRenderer.color = _activeColor;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _spriteRenderer.color = Color.white;
            }
        }
    }
}