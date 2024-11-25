using DG.Tweening;
using TMPro;
using UnityEngine;

public class TriggerDissapear : MonoBehaviour
{ 
    [SerializeField] private SpriteRenderer[] _spriteRenderers;
    private float _fadeTime = 0.220f;
    private Sequence _appear;

    private void OnTriggerEnter2D(Collider2D other)
    {
        FadeSprites(0.8f);
       
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        FadeSprites(0);
    }
    
    private void FadeSprites(float alphaToFadeTo)
    {
        _appear?.Kill();
        _appear = DOTween.Sequence();
        foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
        {
            _appear.Append(spriteRenderer.DOFade(alphaToFadeTo, _fadeTime));
        }
    }

}
