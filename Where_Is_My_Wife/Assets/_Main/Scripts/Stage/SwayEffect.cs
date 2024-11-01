using DG.Tweening;
using UnityEngine;

namespace WhereIsMyWife.Stage
{
    public class SwayEffect : MonoBehaviour
    {
        [SerializeField] private float _swayDistance = 2f; 
        [SerializeField] private float _cycleDuration = 1f;
        [SerializeField] private Ease _ease = Ease.InOutSine;

        void Start()
        {
            Vector3 originalPosition = transform.position;
            Vector3 startPosition = originalPosition;
            startPosition.x = _swayDistance;
            transform.position = startPosition;
            
            Sequence swaySequence = DOTween.Sequence();
            
            swaySequence.Append(transform.DOMoveX(originalPosition.x - _swayDistance, _cycleDuration / 2).SetEase(_ease)).WaitForCompletion();
            swaySequence.Append(transform.DOMoveX(originalPosition.x + _swayDistance, _cycleDuration / 2).SetEase(_ease)).WaitForCompletion();
            
            swaySequence.SetLoops(-1, LoopType.Restart); 
        }
    }
}
