using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class DoubleJumpTrigger : MonoBehaviour
    {
        [SerializeField] private Color _activeColor = Color.green;
        [SerializeField] private float _respawnDelay = 5f;
        [SerializeField] private float _respawnFadeDuration = 0.5f;
        
        private Collider2D _collider;
        private SpriteRenderer _spriteRenderer;
        private Tween _respawnTween;
        
        private bool _isActive = false;
        
        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        private void Start()
        {
            PlayerManager.Instance.RespawnStartAction += Respawn;
            PlayerManager.Instance.JumpStart += CheckForUse;
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= Respawn;
            PlayerManager.Instance.JumpStart -= CheckForUse;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isActive = true;
                _spriteRenderer.color = _activeColor;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                _isActive = false;
                _spriteRenderer.color = Color.white;
            }
        }

        private void CheckForUse(float _)
        {
            if (_isActive)
            {
                Despawn();
            }
        }
        
        private void Despawn()
        {
            _collider.enabled = false;
            _spriteRenderer.enabled = false;
            
            _respawnTween = DOVirtual.DelayedCall(_respawnDelay, Respawn);
        }
        
        private void Respawn(Vector3 _)
        {
            Respawn();
        }
        
        private void Respawn()
        {
            _respawnTween?.Kill();
            
            Color color = Color.white;
            color.a = 0f;
            
            _spriteRenderer.color = color;
            _spriteRenderer.enabled = true;
            
            _spriteRenderer.DOFade(1, _respawnFadeDuration)
                .OnComplete(() => _collider.enabled = true);
        }
    }
}