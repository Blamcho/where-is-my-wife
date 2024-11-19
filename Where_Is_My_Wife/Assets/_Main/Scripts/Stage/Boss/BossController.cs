using DG.Tweening;
using UnityEngine;
using WhereIsMyWife.Managers;
using Random = UnityEngine.Random;

namespace WhereIsMyWife.Controllers
{
    public class BossController : MonoBehaviour
    {
        [Header("TakingDamage")]
        [SerializeField] private Color _damageColor;
        [SerializeField] private float _damageDuration = 0.1f;
        [SerializeField] private float _damageScaleMultiplier = 2f;
        
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

        [Header("Final Phase")] 
        [SerializeField] private EnemyController _normalTire;
        [SerializeField] private EnemyController _finalTire;
        [SerializeField] private Transform _leftSpawnTransform;
        [SerializeField] private Transform _rightSpawnTransform;
        [SerializeField] private float[] _tireSpawnIntervals;
        [SerializeField] private float _finalSwayDistance = 5f;
        [SerializeField] private float _finalCycleDuration = 0.5f;
        [SerializeField] private float _finalSpawnMultiplier = 2f;
        
        [Header("Scene Flow")]
        [SerializeField] private string _storyEndSceneName = "Story5";
        [SerializeField] private int _currentLevelNumber = 5;
        [SerializeField] private string _currentLevelInitialScene = "Story4";

        [Header("Ending")] 
        [SerializeField] private GameObject[] _hazards;
        [SerializeField] private Sprite _endSprite;
        [SerializeField] private Transform _endTransform;
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private float _endAnimationDuration = 10f;
        [SerializeField] private float _finalFlashDuration = 3f;
        
        private Vector3 _originalLocalPosition;
        private Material _material;
    
        private Sequence _swaySequence;
        private Sequence _firingSequence;
        private Sequence _resettingSequence;
        private Sequence _finalAttackSequence;
        
        private BossManager _bossManager;
        
        private static readonly int FlashColor = Shader.PropertyToID("_FlashColor");
        private static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");

        private void Awake()
        {
            _material = GetComponent<SpriteRenderer>().material;
            _originalLocalPosition = transform.localPosition;
        }

        private void Start()
        {
            _bossManager = BossManager.Instance;
            _bossManager.StartFiringEvent += StartFiring;
            _bossManager.StopFiringEvent += StopFiring;
            _bossManager.StartSwayingEvent += StartSwaying;
            _bossManager.StopSwayingEvent += StartIdling;
            _bossManager.StartFinalPhaseEvent += StartFinalPhase;
            _bossManager.StartFinalAttackEvent += StartFinalAttack;
            _bossManager.StopFinalAttackEvent += StopFinalAttack;
            _bossManager.TakeDamageEvent += TakeDamage;
            _bossManager.DieEvent += Die;
            
            _material.SetColor(FlashColor, _damageColor);
        }

        private void OnDestroy()
        {
            _bossManager.StartFiringEvent -= StartFiring;
            _bossManager.StopFiringEvent -= StopFiring;
            _bossManager.StartSwayingEvent -= StartSwaying;
            _bossManager.StopSwayingEvent -= StartIdling;
            _bossManager.StartFinalPhaseEvent -= StartFinalPhase;
            _bossManager.StartFinalAttackEvent -= StartFinalAttack;
            _bossManager.StopFinalAttackEvent -= StopFinalAttack;
            _bossManager.TakeDamageEvent -= TakeDamage;
            _bossManager.DieEvent -= Die;
        }

        private void StartFinalPhase()
        {
            _swayDistance = _finalSwayDistance;
            _cycleDuration = _finalCycleDuration;
            _minProjectileSpawnInterval /= _finalSpawnMultiplier;
            _maxProjectileSpawnInterval /= _finalSpawnMultiplier;
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

            _resettingSequence.Append(_swaySequence.SetLoops(int.MaxValue));
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

            _resettingSequence.Append(_swaySequence.SetLoops(int.MaxValue, LoopType.Yoyo));
        }

        private void StartFinalAttack()
        {
            _finalAttackSequence = DOTween.Sequence();
            
            for (int i = 0; i < _tireSpawnIntervals.Length; i++)
            {
                var isFinalInterval = i == _tireSpawnIntervals.Length - 1;
                var spawnFromRight = CoinFlip();
                var spawnPosition = spawnFromRight ? _rightSpawnTransform.position : _leftSpawnTransform.position;
                
                _finalAttackSequence.AppendInterval(_tireSpawnIntervals[i]);
                
                _finalAttackSequence.AppendCallback(() =>
                {
                    EnemyController tireToSpawn = isFinalInterval ? _finalTire : _normalTire;
                    
                    tireToSpawn.Activate(spawnPosition);
                });
            }
        }

        private void StopFinalAttack()
        {
            _finalAttackSequence?.Kill();
        }

        private static bool CoinFlip()
        {
            return Random.value >= 0.5f;
        }

       private void TakeDamage()
       {
            _material.DOFloat(1, FlashAmount, _damageDuration / 2)
                .OnComplete(() => _material.DOFloat(0, FlashAmount, _damageDuration / 2));
            transform.DOPunchScale(Vector3.one * _damageScaleMultiplier, _damageDuration);
       }
       
       private void Die()
       {
           StopFiring();
           _resettingSequence?.Kill();
           StopFinalAttack();

           foreach (GameObject hazard in _hazards)
           {
               Destroy(hazard);
           }
           
           AudioManager.Instance.StopMusic(true);
           _material.SetColor(FlashColor, Color.white);
           
           Sequence sequence = DOTween.Sequence();

           sequence.Append(transform.DOLocalMoveX(0, 1f).SetEase(_ease));
           sequence.AppendInterval(3f);
           sequence.Append(_material.DOFloat(1, FlashAmount, _finalFlashDuration / 2));
           sequence.AppendCallback(() => { GetComponent<SpriteRenderer>().sprite = _endSprite; });
           sequence.Append(_material.DOFloat(0, FlashAmount, _finalFlashDuration / 2));
           sequence.AppendCallback(() => { AudioManager.Instance.PlayMusic("FinalAnimation"); });
           sequence.AppendInterval(3f);
           sequence.Append(transform.DOMove(_endTransform.position, _endAnimationDuration));
           sequence.AppendCallback(() => { _particleSystem.Stop(); });
           sequence.AppendInterval(3f);
           sequence.AppendCallback(() =>
               {
                   if (LevelManager.Instance.IsInStoryMode)
                   {
                       DataSaveManager.Instance.SetNextLevelParameters(_currentLevelNumber, _currentLevelInitialScene, true);
                   }
           
                   LevelManager.Instance.LoadScene(_storyEndSceneName);
               });
       }
    }
}
