using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Plays audio feedback for UI button interactions in the ODON application.
/// Handles hover and click sounds using AudioSource and AudioClip components.
/// </summary>
[Obsolete("UIButtonAudio is deprecated.")]
public class UIButtonAudio : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    /// <summary>
    /// Enables or disables hover sound playback.
    /// </summary>
    [SerializeField] private bool hoverSound = true;

    /// <summary>
    /// Enables or disables click sound playback.
    /// </summary>
    [SerializeField] private bool clickSound = true;

    /// <summary>
    /// AudioClip played when the button is hovered.
    /// </summary>
    [SerializeField] private AudioClip clipHover;

    /// <summary>
    /// AudioClip played when the button is clicked.
    /// </summary>
    [SerializeField] private AudioClip clipClick;

    /// <summary>
    /// The AudioSource component used to play button sounds.
    /// </summary>
    private AudioSource buttonAudioSource;

    /// <summary>
    /// Initializes the AudioSource component.
    /// </summary>
    private void Start()
    {
        buttonAudioSource = transform.gameObject.AddComponent<AudioSource>();
    }

    /// <summary>
    /// Plays the hover sound when the pointer enters the button.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverSound) PlaySound(clipHover);
    }

    /// <summary>
    /// Plays the click sound when the button is clicked.
    /// </summary>
    /// <param name="eventData">Pointer event data.</param>
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (clickSound) PlaySound(clipClick);
    }

    /// <summary>
    /// Plays the specified audio clip using the button's AudioSource.
    /// </summary>
    /// <param name="clip">The AudioClip to play.</param>
    private void PlaySound(AudioClip clip)
    {
        buttonAudioSource.clip = clip;
        buttonAudioSource.Play();
    }
}
