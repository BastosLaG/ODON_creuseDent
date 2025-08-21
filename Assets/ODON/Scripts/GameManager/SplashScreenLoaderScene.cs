using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ODON.GameManager
{
    public class SplashScreenLoaderScene : MonoBehaviour
    {
        [SerializeField] private float delay = 6;
        private void Start()
        {
            StartCoroutine(LoadingScene());
        }
        IEnumerator LoadingScene ()
        {
            yield return new WaitForSeconds(delay);
                SceneManager.LoadScene("MainMenu");
        }
    }
}