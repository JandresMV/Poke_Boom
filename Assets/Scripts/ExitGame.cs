using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Salir del juego."); // Esto se ver� en la consola solo en el editor.
    }
}

