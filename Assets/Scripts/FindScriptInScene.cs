using UnityEngine;

public class FindScriptInScene : MonoBehaviour
{
    public string scriptNameToFind; // Nombre del script que quieres buscar

    void Start()
    {
        // Obtener todos los objetos de la escena
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        bool found = false;

        foreach (GameObject obj in allObjects)
        {
            // Verificar si el objeto tiene un componente con el nombre del script
            Component script = obj.GetComponent(scriptNameToFind);

            if (script != null)
            {
                found = true;
                Debug.Log($"El script '{scriptNameToFind}' está en el objeto '{obj.name}'.");
            }
        }

        if (!found)
        {
            Debug.Log($"El script '{scriptNameToFind}' no está añadido a ningún objeto en la escena.");
        }
    }
}
