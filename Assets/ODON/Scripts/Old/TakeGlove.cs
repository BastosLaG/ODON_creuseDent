using UnityEngine;

/// <summary>
/// Handles the glove-taking interaction in the ODON application.
/// Plays glove animations and manages timing for animation and destruction.
/// </summary>
public class TakeGlove : MonoBehaviour
{
    /// <summary>
    /// Array of Animation components for glove animations.
    /// </summary>
    [SerializeField] private Animation[] gloveAnim;

    /// <summary>
    /// Delay before playing the glove animation again, configurable from the inspector.
    /// </summary>
    [SerializeField] private float startDelay = 1f;

    /// <summary>
    /// Delay before destroying the glove object, configurable from the inspector.
    /// </summary>
    [SerializeField] private float DestroyRate = 2f;

    /// <summary>
    /// Plays all glove animations.
    /// </summary>
    private void PlayTakeGlove()
    {
        foreach (Animation anim in gloveAnim)
        {
            anim.Play();
        }
    }

    /// <summary>
    /// Initiates the glove-taking process, plays animations, and schedules destruction.
    /// </summary>
    public void TakeGloves()
    {
        PlayTakeGlove();
        Invoke(nameof(PlayTakeGlove), startDelay);
        Invoke(nameof(Destroy), DestroyRate);
    }
}