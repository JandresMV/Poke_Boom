using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject nextButton; // Referencia al bot�n que se mostrar�
    private Vector3 originalScale; // Tama�o original del personaje
    private static CharacterSelector selectedCharacter; // Referencia al personaje seleccionado actualmente

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
        if (nextButton != null)
        {
            nextButton.SetActive(false); // Aseg�rate de que el bot�n est� desactivado al inicio
        }
    }

    void OnMouseDown()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }

        if (selectedCharacter != null && selectedCharacter != this)
        {
            selectedCharacter.ResetScale();
        }

        IncreaseScale();
        selectedCharacter = this;

        if (nextButton != null)
        {
            nextButton.SetActive(true); // Muestra el bot�n al hacer clic
        }

        // Guardar la selecci�n del personaje
        PlayerPrefs.SetString("SelectedCharacter", gameObject.name);

        // Debug para verificar el nombre del personaje seleccionado
        Debug.Log("Selected Character Name: " + gameObject.name);
    }

    private void IncreaseScale()
    {
        transform.localScale = originalScale * 1.5f; // Aumenta el tama�o un 50%
    }

    private void ResetScale()
    {
        transform.localScale = originalScale; // Restablece el tama�o original
    }
}
