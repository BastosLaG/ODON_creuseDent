using UnityEngine;
using UnityEngine.Video;

/// <summary>
/// Lecture d'un fichier vidéo depuis le dossier StreamingAssets à l'aide d'un composant <see cref="VideoPlayer"/>.
/// </summary>
/// <remarks>
/// Ce script doit être attaché à un GameObject contenant un composant <see cref="VideoPlayer"/>.
/// Le nom du fichier vidéo doit être défini via l'inspecteur Unity.
/// </remarks>
public class VideoClipPlayerURL : MonoBehaviour
{
    /// <summary>
    /// Nom du fichier vidéo à lire (présent dans le dossier StreamingAssets).
    /// </summary>
    [SerializeField] 
    string videoFileName;

    /// <summary>
    /// Appelé au démarrage du jeu : lance automatiquement la lecture de la vidéo.
    /// </summary>
    void Start()
    {
        PlayVideo();
    }

    /// <summary>
    /// Configure le <see cref="VideoPlayer"/> pour lire la vidéo spécifiée par <see cref="videoFileName"/>.
    /// </summary>
    /// <remarks>
    /// Vérifie d'abord la présence d'un composant <see cref="VideoPlayer"/> sur le GameObject.
    /// Si aucun composant n'est trouvé, un message d'erreur est affiché dans la console.
    /// </remarks>
    public void PlayVideo()
    {
        if (!TryGetComponent<VideoPlayer>(out var videoPlayer))
        {
            Debug.LogError("No VideoPlayer component found on this GameObject.");
            return;
        }

        // Construit le chemin complet vers le fichier vidéo dans StreamingAssets
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);

        // Affiche dans la console le chemin utilisé
        // Debug.Log("Playing video from URL: " + filePath);

        // Associe l'URL au VideoPlayer et démarre la lecture
        videoPlayer.url = filePath;
        videoPlayer.Play();
    }
}
