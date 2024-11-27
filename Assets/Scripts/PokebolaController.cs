using UnityEngine;
using System.Collections;

public class PokebolaController : MonoBehaviour
{
    public Camera mainCamera;       // C�mara principal
    public GameObject[] characters; // Lista de personajes
    public float fallSpeed = 20f;   // Velocidad de ca�da inicial
    public float bounceForce = 500f; // Fuerza de rebote
    public float autoThrowForce = 700f; // Fuerza de lanzamiento autom�tico
    public float moveSpeed = 10f;   // Velocidad hacia el objetivo
    public Transform groundTransform; // Referencia al suelo

    private Rigidbody pokeballRigidbody;
    private Vector3 initialCameraPosition;
    private bool hasBounced = false;

    void Start()
    {
        pokeballRigidbody = GetComponent<Rigidbody>();

        if (pokeballRigidbody == null)
        {
            Debug.LogError("Pokebola debe tener un Rigidbody.");
            return;
        }

        if (groundTransform == null)
        {
            Debug.LogError("Ground Transform no est� asignado en el Inspector.");
            return;
        }

        // Inicializar como cinem�tico para ca�da controlada
        pokeballRigidbody.useGravity = false;
        pokeballRigidbody.isKinematic = true;

        // Guardar la posici�n inicial de la c�mara
        initialCameraPosition = mainCamera.transform.position;

        StartCoroutine(FallFromSky());
    }

    IEnumerator FallFromSky()
    {
        // Ca�da controlada
        while (transform.position.y > groundTransform.position.y + 0.5f)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            FollowPokeballWithCamera();
            yield return null;
        }

        // Activar f�sica real al tocar el suelo
        pokeballRigidbody.isKinematic = false;
        pokeballRigidbody.useGravity = true;

        // Esperar un instante para estabilizar el rebote
        yield return new WaitForSeconds(0.1f);

        if (!hasBounced)
        {
            hasBounced = true;
            pokeballRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

            // Restaurar la c�mara y lanzar autom�ticamente despu�s del rebote
            StartCoroutine(ReturnCameraToInitialPosition());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(AutoThrow());
        }
    }

    private void FollowPokeballWithCamera()
    {
        // Actualizaci�n de la posici�n de la c�mara para que siga a la Pokebola
        Vector3 targetPosition = transform.position + new Vector3(0, 5, -10); // Ajusta la distancia seg�n sea necesario
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 5f);
        mainCamera.transform.LookAt(transform.position); // Asegurar que la c�mara siempre apunte a la Pokebola
    }

    IEnumerator ReturnCameraToInitialPosition()
    {
        // Retornar la c�mara a su posici�n inicial tras el rebote
        float timeElapsed = 0f;
        float duration = 1f;
        Vector3 currentCameraPosition = mainCamera.transform.position;

        while (timeElapsed < duration)
        {
            mainCamera.transform.position = Vector3.Lerp(currentCameraPosition, initialCameraPosition, timeElapsed / duration);
            mainCamera.transform.LookAt(transform.position); // Apuntar a la Pokebola durante el regreso
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        mainCamera.transform.position = initialCameraPosition;
    }

    IEnumerator AutoThrow()
    {
        // Seleccionar un personaje aleatorio como objetivo
        GameObject target = GetRandomTarget();

        while (target != null && Vector3.Distance(transform.position, target.transform.position) > 1f)
        {
            Vector3 direction = (target.transform.position - transform.position).normalized;
            pokeballRigidbody.velocity = direction * moveSpeed;
            yield return null;
        }

        if (target != null)
        {
            Debug.Log("Pokebola alcanz� a " + target.name);
        }
    }

    private GameObject GetRandomTarget()
    {
        // Elegir un personaje aleatorio de la lista
        return characters[Random.Range(0, characters.Length)];
    }
}
