using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int selectedCharacterIndex = -1; // Índice del personaje seleccionado

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // Persistir entre escenas
    }
}
