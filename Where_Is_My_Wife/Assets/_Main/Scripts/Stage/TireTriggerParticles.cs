using UnityEngine;

public class TireTriggerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject _parentGameObject;
    
    private void Awake()
    {
        var shape = _particleSystem.shape;
        shape.radius = _parentGameObject.transform.localScale.x / 2;
        
        var emission = _particleSystem.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(emission.rateOverTime.constant * _parentGameObject.transform.localScale.x);
    }
}
