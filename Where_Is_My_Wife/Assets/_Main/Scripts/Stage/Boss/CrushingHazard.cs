using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class CrushingHazard : MonoBehaviour
    {
        [SerializeField] private Transform _objectPivot;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Collider2D _hazardCollider;
        [SerializeField] private float _fadeTime;
        [SerializeField] private float _finalAlpha = 0.5f;
        [SerializeField] private float _smashAnticipationTime;
        [SerializeField] private float _smashActionTime;
        [SerializeField] private float _shakeStrength;
        
        private Vector2 _originalPosition;
        private bool _isActive = false;

        private void Awake()
        {
            _originalPosition = _objectPivot.position;
        }

        private void Start()
        {
            PlayerManager.Instance.RespawnStartAction += Deactivate;
        }

        private void OnDestroy()
        {
            PlayerManager.Instance.RespawnStartAction -= Deactivate;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isActive && other.CompareTag("Player"))
            {
                Activate();
            }
        }

        private void Activate()
        {
            _isActive = true;

            AudioManager.Instance.PlaySFX("ManotazoWindUp");
            
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_objectPivot.DOShakePosition(_smashAnticipationTime, _shakeStrength))
                .Join(_spriteRenderer.DOFade(_finalAlpha, _fadeTime));
            sequence.AppendCallback(() =>
            {
                _hazardCollider.enabled = true; 
                AudioManager.Instance.PlaySFX("ManotazoHit");
            });
            sequence.Append(_objectPivot.DOMoveY(transform.position.y, _smashActionTime));
            sequence.AppendCallback(() => { _hazardCollider.enabled = false; });
            sequence.Append(_spriteRenderer.DOFade(0f, _smashAnticipationTime));

            Deactivate();
        }
        
        private void Deactivate()
        {
            _isActive = false;
            _hazardCollider.enabled = false;
            _objectPivot.position = _originalPosition;
        }

        private void Deactivate(Vector3 _)
        {
            Deactivate();
        }
    }
}
