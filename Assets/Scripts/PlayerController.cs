using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public bool isPlayerSelected; // Determina si este es el jugador controlado
    public bool hasPokeball; // Indica si tiene la Pokebola
    public GameObject pokeball; // Referencia a la Pokebola
    public Transform pokeballPosition; // Posici�n fija frente al personaje
    public float throwForce = 700f; // Fuerza de lanzamiento
    public GameObject[] targetPositions; // Objetos vac�os como objetivos en el escenario
    public float minThrowDelay = 0f; // M�nimo tiempo de retraso para la IA
    public float maxThrowDelay = 2f; // M�ximo tiempo de retraso para la IA

    private bool isAITaskRunning = false; // Evitar m�ltiples tareas para la IA

    void Start()
    {
        // Obtener el nombre del personaje seleccionado
        string selectedCharacterName = PlayerPrefs.GetString("SelectedCharacter");

        // Verificar si este personaje es el seleccionado
        if (gameObject.name == selectedCharacterName)
        {
            isPlayerSelected = true;
            Debug.Log("Player seleccionado: " + gameObject.name);
        }
    }

    void Update()
    {
        if (isPlayerSelected && hasPokeball)
        {
            HandlePlayerThrow();
        }
        else if (!isPlayerSelected && hasPokeball && !isAITaskRunning)
        {
            StartCoroutine(AutoThrowForAI());
        }
    }

    // M�todo para manejar el lanzamiento manual del jugador
    private void HandlePlayerThrow()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ThrowPokeballToTarget(GetTargetPosition(Vector3.forward));
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ThrowPokeballToTarget(GetTargetPosition(Vector3.back));
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ThrowPokeballToTarget(GetTargetPosition(Vector3.left));
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ThrowPokeballToTarget(GetTargetPosition(Vector3.right));
        }
    }

    // Corrutina para que los jugadores IA lancen la Pokebola de manera autom�tica
    private IEnumerator AutoThrowForAI()
    {
        isAITaskRunning = true;

        // Tiempo aleatorio antes de lanzar
        float delay = Random.Range(minThrowDelay, maxThrowDelay);
        yield return new WaitForSeconds(delay);

        // Elegir un objetivo aleatorio
        GameObject randomTarget = targetPositions[Random.Range(0, targetPositions.Length)];
        ThrowPokeballToTarget(randomTarget);

        isAITaskRunning = false;
    }

    // M�todo para lanzar la Pokebola hacia un objetivo
    private void ThrowPokeballToTarget(GameObject target)
    {
        if (target != null && hasPokeball)
        {
            hasPokeball = false;
            pokeball.transform.parent = null; // Desvincular la Pokebola
            Rigidbody rb = pokeball.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.velocity = Vector3.zero; // Limpiar velocidad previa

                // Calcular direcci�n hacia el objetivo
                Vector3 direction = (target.transform.position - transform.position).normalized;
                rb.AddForce(direction * throwForce);
            }
        }
    }

    // M�todo para obtener el objetivo m�s cercano en una direcci�n espec�fica
    private GameObject GetTargetPosition(Vector3 direction)
    {
        GameObject closestTarget = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject target in targetPositions)
        {
            Vector3 targetDirection = (target.transform.position - transform.position).normalized;
            float dotProduct = Vector3.Dot(direction, targetDirection);

            if (dotProduct > 0) // Asegurarse de que el objetivo est� en la direcci�n general
            {
                float distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        return closestTarget;
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

                // Fijar la Pokebola frente al jugador
                pokeball.transform.parent = transform;
                pokeball.transform.position = pokeballPosition.position;
                rb.velocity = Vector3.zero; // Detener cualquier movimiento residual
            }
        }
    }
}
