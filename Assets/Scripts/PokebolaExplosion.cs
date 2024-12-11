using UnityEngine;
using UnityEngine.UI; // Necesario para manejar elementos de UI
using UnityEngine.SceneManagement;

public class PokebolaExplosion : MonoBehaviour
{
    public GameObject explosionPrefab; // Prefab de la explosi�n
    public float minTimeToExplode = 2f; // Tiempo m�nimo para explotar
    public float maxTimeToExplode = 5f; // Tiempo m�ximo para explotar
    public AudioClip explosionSound; // Clip de audio para la explosi�n
    public float explosionScale = 2f; // Escala de la explosi�n

    public Canvas uiCanvas; // Canvas para mostrar el texto y botones
    public Text resultText; // Texto para mostrar el mensaje del jugador
    public Button restartButton; // Bot�n para reiniciar el juego
    public Button exitButton; // Bot�n para salir del juego

    private float countdown; // Contador de tiempo para la explosi�n
    private AudioSource audioSource; // Fuente de audio para reproducir sonidos
    private GameObject lastPlayer; // �ltimo jugador que tuvo la Pokebola

    void Start()
    {
        // Configurar tiempo de explosi�n aleatorio
        countdown = Random.Range(minTimeToExplode, maxTimeToExplode);

        // Obtener o agregar un AudioSource al objeto
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Ocultar la UI al inicio
        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Reducir el contador
        countdown -= Time.deltaTime;

        // Verificar si es momento de explotar
        if (countdown <= 0)
        {
            Explode();
        }
    }

    void Explode()
    {
        // Reproducir el sonido de la explosi�n
        if (explosionSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(explosionSound);
        }

        // Instanciar el prefab de la explosi�n en la posici�n de la Pokebola
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            explosion.transform.localScale *= explosionScale;
        }

        // Mostrar el mensaje de derrota
        if (lastPlayer != null && resultText != null)
        {
            resultText.text = $"El jugador {lastPlayer.name} HA PERDIDO";
        }

        // Mostrar la UI con texto y botones
        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(true);
        }

        // Destruir la Pokebola tras la explosi�n
        Destroy(gameObject, 0.5f); // Breve retraso para que el sonido se reproduzca antes de destruir
    }

    private void OnTriggerEnter(Collider other)
    {
        // Guardar al jugador que tuvo la Pokebola
        if (other.CompareTag("Player"))
        {
            lastPlayer = other.gameObject;
        }
    }

    // M�todos para los botones
    public void RestartGame()
    {
        // Cargar la escena inicial
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        // Salir del juego
        Application.Quit();
        Debug.Log("Salir del juego."); // Esto no se ejecutar� en el editor
    }
}
