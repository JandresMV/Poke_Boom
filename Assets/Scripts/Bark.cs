using UnityEngine;

public class PlaySoundOnSpace : MonoBehaviour
{
    public AudioClip sonido; // Aqu� arrastrar�s el AudioClip desde el Project

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(sonido, transform.position);
        }
    }
}