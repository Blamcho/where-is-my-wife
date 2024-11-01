using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;

public class BossController : MonoBehaviour
{
    [Header("Swaying")]
    [SerializeField] private float _swayDistance = 2f; 
    [SerializeField] private float _cycleDuration = 1f;
    [SerializeField] private Ease _ease = Ease.InOutSine;
    
    [Header("Projectile Spawning")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private float _minProjectileSpawnInterval = 1f;
    [SerializeField] private float _maxProjectileSpawnInterval = 3f;

    private Vector3 _originalPosition; 
    
    private Sequence _swaySequence;
    private Sequence _firingSequence;
    
    private BossManager _bossManager;
    
    private void Start()
    {
        _bossManager = BossManager.Instance;
        _bossManager.StartFiringEvent += StartFiring;
        _bossManager.StopFiringEvent += StopFiring;
        _bossManager.StartSwayingEvent += StartSwaying;
        _bossManager.StopSwayingEvent += StopSwaying;
    }

    private void OnDestroy()
    {
        _bossManager.StartFiringEvent -= StartFiring;
        _bossManager.StopFiringEvent -= StopFiring;
        _bossManager.StartSwayingEvent -= StartSwaying;
        _bossManager.StopSwayingEvent -= StopSwaying;
    }

    private void StartFiring()
    {
        _firingSequence = DOTween.Sequence();

        _firingSequence.AppendInterval(Random.Range(_minProjectileSpawnInterval, _maxProjectileSpawnInterval));
        
        _firingSequence.AppendCallback(() =>
        {
            Instantiate(_projectilePrefab, transform.position, Quaternion.identity);
        });

        _firingSequence.SetLoops(-1, LoopType.Restart);
    }

    private void StopFiring()
    {
        _firingSequence.Kill();
    }

    private void StartSwaying()
    {
        Vector3 startPosition = transform.localPosition;
        startPosition.x = _swayDistance;
        transform.localPosition = startPosition;
        
        _swaySequence = DOTween.Sequence();
        
        _swaySequence.Append(transform.DOLocalMoveX(-_swayDistance, _cycleDuration / 2).SetEase(_ease));
        _swaySequence.Append(transform.DOLocalMoveX(_swayDistance, _cycleDuration / 2).SetEase(_ease));
        
        _swaySequence.SetLoops(-1, LoopType.Restart);
    }

    private void StopSwaying()
    {
        _swaySequence.Kill();
        
        transform.DOLocalMoveX(0, 0.5f).SetEase(_ease);
    }
}
