using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.Platforms_fade_away
{
    public class CrumblingPlatform : MonoBehaviour
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

        void Start()
        {
            _platformCollider = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (_isPlayerOnPlatform)
            {
                _timeOnPlatform += Time.deltaTime;
                if (_timeOnPlatform >= _timeBeforeFade)
                {
                    float alpha = Mathf.Lerp(1f, 0f, (_timeOnPlatform - _timeBeforeFade) / _fadeDuration);
                    _spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
                    if (alpha <= 0f)
                    {
                        _spriteRenderer.enabled = false;
                        _platformCollider.enabled = false;
                        StartCoroutine(RespawnPlatform());
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

        private IEnumerator RespawnPlatform()
        {
            yield return new WaitForSeconds(_respawnTime);
            _shakeTween.Kill();
            _isPlayerOnPlatform = false;
            _spriteRenderer.enabled = true;
            _platformCollider.enabled = true;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _timeOnPlatform = 0f;
        }
    }
}
