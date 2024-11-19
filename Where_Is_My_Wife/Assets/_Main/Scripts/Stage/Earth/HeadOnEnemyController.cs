using DG.Tweening;
using UnityEngine;

public class HeadOnEnemyController : EnemyController
{
    [SerializeField] private float _timeToReachYPosition;
    [SerializeField] private float _timeToReachHorizontalPosition;
    [SerializeField] private Ease _horizontalEase = Ease.InSine;
    
    private TrailRenderer _trailRenderer;
    private Tweener _verticalTweener;
    private Tween _horizontalDistanceTween;
    
    private float _horizontalDistance;
    private float _originalTrailFadeTime;

    private void Awake()
    {
        _trailRenderer = GetComponent<TrailRenderer>();
        _originalTrailFadeTime = _trailRenderer.time;
    }

    public override void Activate(Vector2 position)
    {
        base.Activate(position);
        _trailRenderer.Clear();
        StartMovingTowardsPlayer();
    }

    public override void Deactivate()
    {
        StopMovingTowardsPlayer();
        base.Deactivate();
    }

    protected override void Pause()
    {
        base.Pause();
        _trailRenderer.time = float.MaxValue;
    }
    
    protected override void Resume()
    {
        base.Resume();
        _trailRenderer.time = _originalTrailFadeTime;
    }

    private void StopMovingTowardsPlayer()
    {
        _isMovingTowardsPlayer = false;
        _verticalTweener.Kill();
        _horizontalDistanceTween.Kill();
    }

    public void StartMovingTowardsPlayer()
    {
        _isMovingTowardsPlayer = true;
        UpdatePlayerPosition();
        
        StartHorizontalDistanceTween();
        StartVerticalTweener();
    }

    private void StartHorizontalDistanceTween()
    {
        _horizontalDistance = transform.position.x - _playerPosition.x;
        
        _horizontalDistanceTween = DOTween
            .To(
                () => _horizontalDistance,
                x => _horizontalDistance = x,
                0,
                _timeToReachHorizontalPosition
            ).SetEase(_horizontalEase);

        _horizontalDistanceTween.OnUpdate(
            delegate() { transform.position = new Vector2(_playerPosition.x + _horizontalDistance, transform.position.y);
            });
    }

    private void StartVerticalTweener()
    {
        _verticalTweener = transform.DOMoveY(_playerPosition.y, _timeToReachYPosition);

        _verticalTweener.OnUpdate(delegate()
        {
            if (Vector3.Distance(transform.position, _playerPosition) > 0.1f)
            {
                _verticalTweener.ChangeEndValue((Vector3)_playerPosition, true);
            }
        });
    }
}
