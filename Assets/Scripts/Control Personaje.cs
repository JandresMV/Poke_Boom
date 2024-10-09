using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;  // Velocidad de movimiento
    private Rigidbody2D rb;  // Referencia al Rigidbody2D
    private Vector2 movement;  // Dirección del movimiento

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Asignar el Rigidbody2D
    }

    void Update()
    {
        // Capturar input de los ejes horizontal y vertical
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        // Mover el jugador usando física
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}

