using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;


public class RespawnCurtainsAnimation : MonoBehaviour
{
    [SerializeField] private Image _leftPanel;
    [SerializeField] private Image _rightPanel;
    [SerializeField] private float _animationDuration;
    [SerializeField] private float _closedTime;
    [SerializeField] private Ease _ease = Ease.InSine;

    private float _panelWidth;

    private void Awake()
    {
        _panelWidth = _leftPanel.rectTransform.rect.width;
    }

    private void Start()
    {
        PlayerManager.Instance.DeathAction += StartAnimation;
    }

    private void OnDestroy()
    {
        PlayerManager.Instance.DeathAction -= StartAnimation;
    }

    private void StartAnimation()
    {
        AnimationAsync().Forget();
    }
    
    private async UniTaskVoid AnimationAsync()
    {
        _leftPanel.rectTransform.DOAnchorPosX(_panelWidth, _animationDuration).SetEase(_ease);
        await _rightPanel.rectTransform.DOAnchorPosX(-_panelWidth, _animationDuration)
            .SetEase(_ease).AsyncWaitForCompletion();

        await UniTask.Delay(TimeSpan.FromSeconds(_closedTime));
        
        PlayerManager.Instance.TriggerRespawnStart();
        
        _leftPanel.rectTransform.DOAnchorPosX(0, _animationDuration).SetEase(_ease);
        await _rightPanel.rectTransform.DOAnchorPosX(0, _animationDuration)
            .SetEase(_ease).AsyncWaitForCompletion();
        
        PlayerManager.Instance.TriggerRespawnComplete();
    }
}
