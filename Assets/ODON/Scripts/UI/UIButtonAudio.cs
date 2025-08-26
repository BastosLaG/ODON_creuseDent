using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] private bool hoverSound = true, clickSound = true;
    [SerializeField] private AudioClip clipHover, clipClick;
    private AudioSource buttonAudioSource;
    private void Start()
    {
        buttonAudioSource = transform.gameObject.AddComponent<AudioSource>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound) PlaySound(clipHover);
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (clickSound) PlaySound(clipClick);
    }

    private void PlaySound(AudioClip clip)
    {
        buttonAudioSource.clip = clip;
        buttonAudioSource.Play();
    }

}
