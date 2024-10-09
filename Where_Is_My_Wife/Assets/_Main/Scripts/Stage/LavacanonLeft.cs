using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavacanonLeft : MonoBehaviour
{
    public GameObject lavaProyectilPrefab; 
    public Transform firePoint;            
    public float minFireRate = 2f;         
    public float maxFireRate = 5f;        
    private float nextFireTime = 0f;

    void Start()
    {
      
        nextFireTime = Time.time + Random.Range(minFireRate, maxFireRate);
    }

    void Update()
    {
        if (Time.time >= nextFireTime)
        {
            Disparar();
            nextFireTime = Time.time + Random.Range(minFireRate, maxFireRate);
        }
    }

    void Disparar()
    {
        GameObject proyectil = Instantiate(lavaProyectilPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = proyectil.GetComponent<Rigidbody2D>();
        rb.velocity = -firePoint.right * 5f; 
    }
}
