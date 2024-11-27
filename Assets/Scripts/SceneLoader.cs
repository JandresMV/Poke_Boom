using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public AudioSource audioSource; // Referencia al componente AudioSource
    private string sceneName; // Variable para almacenar el nombre de la escena

    public void LoadScene(string nombre)
    {
        sceneName = nombre; // Almacena el nombre de la escena

        if (audioSource != null)
        {
            audioSource.Play();
        }

        // Cambia de escena después de un pequeño retraso para asegurarte de que el sonido se reproduzca
        Invoke("ChangeScene", audioSource.clip.length);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
