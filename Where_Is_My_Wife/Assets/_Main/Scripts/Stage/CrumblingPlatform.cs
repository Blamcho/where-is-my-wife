using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.Platforms_fade_away
{
    public class CrumblingPlatform : MonoBehaviour
    {
        [SerializeField] private float timeBeforeFade = 2f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private float respawnTime = 5f;
        
        private SpriteRenderer _spriteRenderer;
        private Collider2D _platformCollider;
        private bool _isPlayerOnPlatform = false;
        private float _timeOnPlatform = 0f;

        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _platformCollider = GetComponent<Collider2D>();
        }

        void Update()
        {
            if (_isPlayerOnPlatform)
            {
                _timeOnPlatform += Time.deltaTime;
                if (_timeOnPlatform >= timeBeforeFade)
                {
                    float alpha = Mathf.Lerp(1f, 0f, (_timeOnPlatform - timeBeforeFade) / fadeDuration);
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
                _isPlayerOnPlatform = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                _isPlayerOnPlatform = false;
                _timeOnPlatform = 0f;
                _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }

        private IEnumerator RespawnPlatform()
        {
            yield return new WaitForSeconds(respawnTime);
            _spriteRenderer.enabled = true;
            _platformCollider.enabled = true;
            _spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            _timeOnPlatform = 0f;
        }
    }
}
