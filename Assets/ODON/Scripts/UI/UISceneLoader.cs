using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

namespace ODON.UI
{
    /// <summary>
    /// Manages scene loading in the ODON application, including a loading screen and progress indicators.
    /// Allows switching between game and tutorial scenes based on user input.
    /// </summary>
    public class SceneLoader : MonoBehaviour
    {
        /// <summary>
        /// The GameObject representing the loading screen UI.
        /// </summary>
        public GameObject loadingScreen;

        /// <summary>
        /// The Image component used to display the loading progress.
        /// </summary>
        public Image progressBar;

        /// <summary>
        /// The TextMeshProUGUI component used to display the loading progress percentage.
        /// </summary>
        public TextMeshProUGUI progressText;

        /// <summary>
        /// Toggle UI element to switch between game and tutorial modes.
        /// </summary>
        public Toggle m_Toggle;

        /// <summary>
        /// The name of the game scene.
        /// </summary>
        private string sceneGameName = Data.SceneNames.Game;

        /// <summary>
        /// The name of the tutorial scene.
        /// </summary>
        private string sceneTutorialName = Data.SceneNames.Tutorial;

        /// <summary>
        /// Indicates whether the tutorial mode is active.
        /// </summary>
        private bool isTutorial = false;

        /// <summary>
        /// Gets or sets the current scene name based on the tutorial mode.
        /// </summary>
        public string SceneName
        {
            get { return isTutorial ? sceneTutorialName : sceneGameName; }
            set { sceneGameName = value; }
        }

        /// <summary>
        /// Initializes the scene loader and sets up the toggle listener.
        /// </summary>
        void Start()
        {
            if (m_Toggle != null)
            {
                m_Toggle.onValueChanged.AddListener(delegate
                {
                    ToggleValueChanged(m_Toggle);
                });
                ToggleValueChanged(m_Toggle);
            }
            else
            {
                isTutorial = false;
            }
        }

        /// <summary>
        /// Initiates the scene loading process for the current scene name.
        /// </summary>
        public void LoadScene()
        {
            StartCoroutine(LoadSceneAsync(SceneName));
        }

        /// <summary>
        /// Initiates the scene loading process for a specified scene name.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        public void LoadScene(string sceneName)
        {
            SceneName = sceneName;
            StartCoroutine(LoadSceneAsync(SceneName));
        }

        /// <summary>
        /// Asynchronously loads the specified scene and updates the loading screen and progress indicators.
        /// </summary>
        /// <param name="sceneName">The name of the scene to load.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        IEnumerator LoadSceneAsync(string sceneName)
        {
            if (loadingScreen != null)
                loadingScreen.SetActive(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            operation.allowSceneActivation = false;

            while (!operation.isDone)
            {
                float progress = Mathf.Clamp01(operation.progress / 0.9f);
                if (progressBar != null)
                    progressBar.fillAmount = progress;

                if (progressText != null)
                    progressText.text = "Chargement en cours... " + Mathf.RoundToInt(progress * 100f) + "%";

                if (operation.progress >= 0.9f)
                {
                    yield return new WaitForSeconds(0.5f);
                    operation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        /// <summary>
        /// Updates the tutorial mode state based on the toggle value.
        /// </summary>
        /// <param name="change">The Toggle component that triggered the change.</param>
        public void ToggleValueChanged(Toggle change)
        {
            isTutorial = change.isOn;
        }
    }
}