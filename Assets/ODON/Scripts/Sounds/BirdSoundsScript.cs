using UnityEngine;

namespace ODON.Sounds
{
    public class BirdSoundsScript : MonoBehaviour
    {
        [SerializeField] private AudioSource[] birdSounds;
        [SerializeField] private float minSoundInterval = 5f;
        [SerializeField] private float maxSoundInterval = 15f;
        private float soundTimer;

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