using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Game;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Platforms_fade_away
{
    public class CrumblingPlatform : PausableMonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private float _shakeStrength = 1f;
        [SerializeField] private float _timeBeforeFade = 2f;
        [SerializeField] private float _fadeDuration = 1f;
        [SerializeField] private float _respawnTime = 5f;
        [SerializeField] private bool _shouldTimerResetsWhenPlayerLeavesPlatform = true;

        private Tween _shakeTween;
        private Collider2D _platformCollider;
        private bool _isPlayerOnPlatform = false;
        private float _timeOnPlatform = 0f;
        private bool _isActive = true;

        protected override void Start()
        {
            base.Start();
            
            PlayerManager.Instance.RespawnStartAction += RespawnPlatform;
            
            _platformCollider = GetComponent<Collider2D>();
        }

        protected override void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= RespawnPlatform;
            
            base.OnDestroy();
        }

        protected override void OnUpdate()
        {
            if (_isPlayerOnPlatform)
            {
                _timeOnPlatform += Time.deltaTime;
                if (_timeOnPlatform >= _timeBeforeFade)
                {
                    float alpha = Mathf.Lerp(1f, 0f, (_timeOnPlatform - _timeBeforeFade) / _fadeDuration);
                    _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                    if (alpha <= 0f && _isActive)
                    {
                        _isActive = false;
                        _spriteRenderer.enabled = false;
                        _platformCollider.enabled = false;
                        
                        DOVirtual.DelayedCall(_respawnTime, RespawnPlatform);
                    }
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _shakeTween = _spriteRenderer.transform.DOShakePosition(_timeBeforeFade, _shakeStrength);
                _isPlayerOnPlatform = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && _shouldTimerResetsWhenPlayerLeavesPlatform)
            {
                _shakeTween.Kill();
                _isPlayerOnPlatform = false;
                _timeOnPlatform = 0f;
                _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        private void RespawnPlatform(Vector3 _)
        {
            RespawnPlatform();
        }
        
        private void RespawnPlatform()
        {
            _isActive = true;
            _shakeTween.Kill();
            _isPlayerOnPlatform = false;
            _spriteRenderer.enabled = true;
            _platformCollider.enabled = true;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _timeOnPlatform = 0f;
        }
    }
}
