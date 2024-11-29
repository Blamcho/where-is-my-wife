using UnityEngine;

public class WallController : MonoBehaviour
{
    public GameObject wall; // Referencia al objeto del muro
    public Transform closedPosition; // Posición donde el muro "aparece" (cerrado)
    public Transform openPosition; // Posición donde el muro "desaparece" (abierto)
    public float moveSpeed = 5f; // Velocidad del movimiento del muro
    private bool isClosing = false; // Estado del movimiento del muro

    private BoxCollider2D wallCollider; // Referencia al BoxCollider2D del muro

    private void Start()
    {
        // Obtener el BoxCollider2D del muro
        wallCollider = wall.GetComponent<BoxCollider2D>();

        if (wallCollider == null)
        {
            Debug.LogError("El muro no tiene un BoxCollider2D asignado.");
            return;
        }

        // Asegurarse de que el muro comience en la posición abierta y con el collider desactivado
        wall.transform.position = openPosition.position;
        wallCollider.enabled = false;
    }

    private void Update()
    {
        // Mover el muro si se está cerrando
        if (isClosing)
        {
            wall.transform.position = Vector3.MoveTowards(
                wall.transform.position,
                closedPosition.position,
                moveSpeed * Time.deltaTime
            );

            // Si el muro llegó a la posición cerrada, activa el BoxCollider2D
            if (Vector3.Distance(wall.transform.position, closedPosition.position) < 0.01f)
            {
                wallCollider.enabled = true;
                isClosing = false; // Detener el movimiento
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isClosing)
        {
            CloseWall();
        }
    }

    public void CloseWall()
    {
        isClosing = true;
        wallCollider.enabled = false; // Asegurarse de que el collider esté desactivado mientras el muro se mueve
    }

    public void OpenWall()
    {
        isClosing = false;
        wall.transform.position = openPosition.position; // Mover instantáneamente a posición abierta
        wallCollider.enabled = false; // Desactivar el collider
    }
}
