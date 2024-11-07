using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Controllers
{
    public class BossController : MonoBehaviour
    {
        [Header("Idle")]
        [SerializeField] private float _idleDistance = 0.5f;
        [SerializeField] private float _idleDuration = 3f;

        [Header("Swaying")]
        [SerializeField] private float _swayDistance = 2f; 
        [SerializeField] private float _cycleDuration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;
    
        [Header("Projectile Spawning")]
        [SerializeField] private GameObject _projectilePrefab;
        [SerializeField] private float _minProjectileSpawnInterval = 1f;
        [SerializeField] private float _maxProjectileSpawnInterval = 3f;

        private Vector3 _originalLocalPosition; 
    
        private Sequence _swaySequence;
        private Sequence _firingSequence;
        private Sequence _resettingSequence;
    
        private BossManager _bossManager;
    
        private void Start()
        {
            _bossManager = BossManager.Instance;
            _bossManager.StartFiringEvent += StartFiring;
            _bossManager.StopFiringEvent += StopFiring;
            _bossManager.StartSwayingEvent += StartSwaying;
            _bossManager.StopSwayingEvent += StartIdling;

            _originalLocalPosition = transform.localPosition;
        }

        private void OnDestroy()
        {
            _bossManager.StartFiringEvent -= StartFiring;
            _bossManager.StopFiringEvent -= StopFiring;
            _bossManager.StartSwayingEvent -= StartSwaying;
            _bossManager.StopSwayingEvent -= StartIdling;
        }

        private void StartFiring()
        {
            _firingSequence = DOTween.Sequence();
        
            _firingSequence.AppendInterval(Random.Range(_minProjectileSpawnInterval, _maxProjectileSpawnInterval));
            _firingSequence.AppendCallback(() => { Instantiate(_projectilePrefab, transform.position, Quaternion.identity);});

            _firingSequence.SetLoops(-1, LoopType.Restart);
        }

        private void StopFiring()
        {
            _firingSequence?.Kill();
        }

        private void StartSwaying()
        {
            _resettingSequence?.Kill();
            _resettingSequence = DOTween.Sequence();

            Vector3 startPosition = _originalLocalPosition;
            startPosition.x = _swayDistance;

            _resettingSequence.Append(transform.DOLocalMove(startPosition, 0.5f).SetEase(_ease));

            _swaySequence = DOTween.Sequence();
            _swaySequence.Append(transform.DOLocalMoveX(-_swayDistance, _cycleDuration / 2).SetEase(_ease));
            _swaySequence.Append(transform.DOLocalMoveX(_swayDistance, _cycleDuration / 2).SetEase(_ease));

            _resettingSequence.Append(_swaySequence.SetLoops(-1));
        }

        private void StartIdling()
        {
            _resettingSequence?.Kill();
            _resettingSequence = DOTween.Sequence();

            Vector3 initialPosition = transform.localPosition;

            _resettingSequence.Append(transform.DOLocalMove(new Vector3(0, initialPosition.y + _idleDistance, initialPosition.z), 0.5f).SetEase(_ease));

            _swaySequence = DOTween.Sequence();
            _swaySequence.Append(transform.DOLocalMoveY(initialPosition.y - _idleDistance, _idleDuration / 2).SetEase(_ease));
            _swaySequence.Append(transform.DOLocalMoveY(initialPosition.y + _idleDistance, _idleDuration / 2).SetEase(_ease));

            _resettingSequence.Append(_swaySequence.SetLoops(-1, LoopType.Yoyo));
        }
    }
}