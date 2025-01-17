using UnityEngine;

[System.Serializable]
public class AudioSound
{
    public string name; // Le nom unique pour identifier ce son
    public AudioClip clip; // Le clip audio
    [Range(0f, 1f)] public float volume = 1f; // Volume du son
    [Range(.1f, 3f)] public float pitch = 1f; // Tonalité
    public bool loop; // Si le son doit être lu en boucle

    [HideInInspector] public AudioSource source; // La source audio utilisée
}
