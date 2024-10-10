using UnityEngine;

public class PlaySoundOnSpace : MonoBehaviour
{
    public AudioClip sonido; // Aquí arrastrarás el AudioClip desde el Project

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(sonido, transform.position);
        }
    }
}