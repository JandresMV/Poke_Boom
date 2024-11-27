using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool isPlayerSelected;   // Determina si este es el jugador controlado
    public bool hasPokeball;        // Indica si tiene la Pokebola
    public GameObject pokeball;    // Referencia a la Pokebola
    public Transform pokeballPosition; // Posición fija frente al personaje
    public float throwForce = 700f; // Fuerza de lanzamiento

    void Update()
    {
        if (isPlayerSelected && hasPokeball)
        {
            HandlePokeballThrow();
        }
    }

    private void HandlePokeballThrow()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ThrowPokeball(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ThrowPokeball(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ThrowPokeball(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ThrowPokeball(Vector3.right);
        }
    }

    private void ThrowPokeball(Vector3 direction)
    {
        hasPokeball = false;
        pokeball.transform.parent = null; // Desvincular la Pokebola
        Rigidbody rb = pokeball.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.velocity = Vector3.zero; // Limpiar velocidad previa
            rb.AddForce(direction * throwForce);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pokeball") && !hasPokeball)
        {
            hasPokeball = true;
            pokeball = collision.gameObject;

            Rigidbody rb = pokeball.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.isKinematic = true;
                pokeball.transform.parent = transform;
                pokeball.transform.position = pokeballPosition.position; // Fijar la posición frente al jugador
                rb.velocity = Vector3.zero; // Detener cualquier movimiento residual
            }
        }
    }
}
