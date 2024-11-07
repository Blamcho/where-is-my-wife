using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using WhereIsMyWife.Managers;

namespace WhereIsMyWife.Stage
{
    public class ChangeStageAnimation : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Transform[] _stageRespawnPositions;
        [SerializeField] private Image _panel;
        [SerializeField] private float _transitionDuration;
        [SerializeField] private Ease _ease;
    
        private void Start()
        {
            BossManager.Instance.GoToNextStageEvent += StartChangeAnimation;
        }

        private void OnDestroy()
        {
            BossManager.Instance.GoToNextStageEvent -= StartChangeAnimation;
        }

        private void StartChangeAnimation(int nextStage)
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(_panel.DOFade(1f, _transitionDuration / 2)).SetEase(_ease);
            sequence.AppendCallback(() =>
            {
                _playerTransform.position = _stageRespawnPositions[nextStage - 1].position;
            });
            sequence.Append(_panel.DOFade(0f, _transitionDuration / 2)).SetEase(_ease);
        }
    }
}
