using UnityEngine;

public class LavaProyectil : MonoBehaviour
{
    public float speed = 5f;          
    public float tiempoDeVida = 8f;   
    private Rigidbody2D rb;

    void Start()
    {
       
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject, tiempoDeVida);
    }
}
