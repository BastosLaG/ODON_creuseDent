using UnityEngine;

namespace ODON.Sounds
{
    /// <summary>
    /// Plays random bird sounds at random intervals in the ODON application.
    /// Manages timing and selection of bird sound effects.
    /// </summary>
    public class BirdSoundsScript : MonoBehaviour
    {
        /// <summary>
        /// Array of AudioSource components for bird sounds.
        /// </summary>
        [SerializeField] private AudioSource[] birdSounds;

        /// <summary>
        /// Minimum interval in seconds between bird sounds.
        /// </summary>
        [SerializeField] private float minSoundInterval = 5f;

        /// <summary>
        /// Maximum interval in seconds between bird sounds.
        /// </summary>
        [SerializeField] private float maxSoundInterval = 15f;

        /// <summary>
        /// Timer tracking time until the next bird sound plays.
        /// </summary>
        private float soundTimer;

        /// <summary>
        /// Updates the timer and plays a random bird sound when the interval elapses.
        /// </summary>
        void Update()
        {
            soundTimer -= Time.deltaTime;
            if (soundTimer <= 0)
            {
                soundTimer = Random.Range(minSoundInterval, maxSoundInterval);
                birdSounds[Random.Range(0, birdSounds.Length)].Play();
            }
        }
    }
}