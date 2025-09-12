using System.Collections;
using ODON.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ODON.GameManager
{
    /// <summary>
    /// Handles the splash screen and loads the main menu scene after a delay in the ODON application.
    /// </summary>
    public class SplashScreenLoaderScene : MonoBehaviour
    {
        /// <summary>
        /// Delay in seconds before loading the main menu scene.
        /// </summary>
        [SerializeField] private float delay = 6;

        /// <summary>
        /// Starts the coroutine to load the main menu scene after the specified delay.
        /// </summary>
        private void Start()
        {
            StartCoroutine(LoadingScene());
        }

        /// <summary>
        /// Coroutine that waits for the specified delay and then loads the main menu scene.
        /// </summary>
        /// <returns>IEnumerator for coroutine.</returns>
        IEnumerator LoadingScene ()
        {
            yield return new WaitForSeconds(delay);
            SceneManager.LoadScene(SceneNames.MainMenu);
        }
    }
}