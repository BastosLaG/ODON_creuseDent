using UnityEngine;
using UnityEngine.Video;

public class VideoClipPlayerURL : MonoBehaviour
{
    [SerializeField] string videoFileName;

    void Start()
    {
        PlayVideo();
    }
    public void PlayVideo()
    {
        if (!TryGetComponent<VideoPlayer>(out var videoPlayer))
        {
            Debug.LogError("No VideoPlayer component found on this GameObject.");
            return;
        }

        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, videoFileName);
        Debug.Log("Playing video from URL: " + filePath);
        videoPlayer.url = filePath;
        videoPlayer.Play();
    }
}
