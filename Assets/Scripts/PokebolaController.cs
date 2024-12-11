using UnityEngine;
using System.Collections;

public class PokebolaController : MonoBehaviour
{
    public Camera mainCamera;       // Cámara principal
    public GameObject[] characters; // Lista de personajes
    public float fallSpeed = 20f;   // Velocidad de caída inicial
    public float bounceForce = 500f; // Fuerza de rebote
    public float autoThrowForce = 700f; // Fuerza de lanzamiento automático
    public float moveSpeed = 10f;   // Velocidad hacia el objetivo
    public Transform groundTransform; // Referencia al suelo

    private Rigidbody pokeballRigidbody;
    private Vector3 initialCameraPosition;
    private bool hasBounced = false;
    private bool isMovingToTarget = false;
    private GameObject targetCharacter;

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
            Debug.LogError("Ground Transform no está asignado en el Inspector.");
            return;
        }

        // Inicializar como cinemático para caída controlada
        pokeballRigidbody.useGravity = false;
        pokeballRigidbody.isKinematic = true;

        // Guardar la posición inicial de la cámara
        initialCameraPosition = mainCamera.transform.position;

        // Obtener el personaje seleccionado
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter");
        targetCharacter = GameObject.Find(selectedCharacterName);

        if (targetCharacter == null)
        {
            Debug.LogError("Personaje seleccionado no encontrado.");
            return;
        }

        StartCoroutine(FallFromSky());
    }

    IEnumerator FallFromSky()
    {
        // Caída controlada
        while (transform.position.y > groundTransform.position.y + 0.5f)
        {
            transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
            FollowPokeballWithCamera();
            yield return null;
        }

        // Activar física real al tocar el suelo
        pokeballRigidbody.isKinematic = false;
        pokeballRigidbody.useGravity = true;

        // Esperar un instante para estabilizar el rebote
        yield return new WaitForSeconds(0.1f);

        if (!hasBounced)
        {
            hasBounced = true;
            pokeballRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);

            // Restaurar la cámara y esperar a que la Pokebola se estabilice después del rebote
            StartCoroutine(ReturnCameraToInitialPosition());
            yield return new WaitForSeconds(0.5f);

            // Mover la Pokebola hacia el personaje seleccionado
            isMovingToTarget = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private void FollowPokeballWithCamera()
    {
        // Actualización de la posición de la cámara para que siga a la Pokebola
        Vector3 targetPosition = transform.position + new Vector3(50, 50, -100); // Ajusta la distancia según sea necesario
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, Time.deltaTime * 2f); // Suavizar el movimiento de la cámara
        mainCamera.transform.LookAt(transform.position); // Asegurar que la cámara siempre apunte a la Pokebola
    }

    IEnumerator ReturnCameraToInitialPosition()
    {
        // Retornar la cámara a su posición inicial tras el rebote
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

    IEnumerator MoveToTarget()
    {
        while (isMovingToTarget && Vector3.Distance(transform.position, targetCharacter.transform.position) > 1f)
        {
            Vector3 direction = (targetCharacter.transform.position - transform.position).normalized;
            pokeballRigidbody.velocity = direction * moveSpeed;
            yield return null;
        }

        if (isMovingToTarget)
        {
            // Detener el movimiento y permitir que el personaje seleccionado controle la Pokebola
            pokeballRigidbody.velocity = Vector3.zero;
            pokeballRigidbody.isKinematic = true;
            isMovingToTarget = false;

            // Asignar la Pokebola al personaje seleccionado
            PlayerController playerController = targetCharacter.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.hasPokeball = true;
                playerController.pokeball = gameObject;
                playerController.pokeballPosition = targetCharacter.transform;
            }
        }
    }
}
