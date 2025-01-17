using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance; // Singleton de l'AudioManager
    public AudioSound[] sounds; // Liste des sons

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject); // Permet à l'AudioManager de persister entre les scènes

        foreach (AudioSound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Son {name} non trouvé !");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Son {name} non trouvé !");
            return;
        }
        s.source.Stop();
    }

    public void SetVolume(string name, float volume)
    {
        AudioSound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Son {name} non trouvé !");
            return;
        }
        s.source.volume = Mathf.Clamp(volume, 0f, 1f);
    }
}
