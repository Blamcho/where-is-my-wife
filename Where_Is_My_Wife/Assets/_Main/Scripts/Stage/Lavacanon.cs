using UnityEngine;
using UnityEngine.Serialization;

namespace WhereIsMyWife.Lavacanon
{
    public class LavaCannon : MonoBehaviour
{
    [SerializeField] private GameObject _lavaProyectilPrefab; 
    [SerializeField] private Transform _firePoint;            
    [SerializeField] private float _minFireRate = 2f;         
    [SerializeField] private float _maxFireRate = 5f;         
    private float _nextFireTime = 0f;
    public bool _shootLeft = true;  

    void Start()
    {
        _nextFireTime = Time.time + Random.Range(_minFireRate, _maxFireRate);
    }

    void Update()
    {
        if (Time.time >= _nextFireTime)
        {
            Shot();
            _nextFireTime = Time.time + Random.Range(_minFireRate, _maxFireRate);
        }
    }

    void Shot()
    {
        GameObject projectile = Instantiate(_lavaProyectilPrefab, _firePoint.position, _firePoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        float direction = _shootLeft ? -1f : 1f;
        rb.velocity = _firePoint.right * direction * 5f; 
    }
}
}
