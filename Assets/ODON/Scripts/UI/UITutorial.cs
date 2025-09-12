using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages the tutorial UI in the ODON application.
/// Displays a series of advice panels to guide the user through the tutorial.
/// </summary>
public class UITutorial : MonoBehaviour
{
    /// <summary>
    /// Reference to the PrimaryButtonWatcher for detecting primary button presses.
    /// </summary>
    public PrimaryButtonWatcher watcher;

    /// <summary>
    /// The GameObject representing the tutorial panel.
    /// </summary>
    [SerializeField] private GameObject TutorialPanel;

    /// <summary>
    /// Array of GameObjects representing individual advice panels.
    /// </summary>
    [SerializeField] private GameObject[] advices;

    /// <summary>
    /// Button used to continue to the next advice.
    /// </summary>
    [SerializeField] private Button continueButton;

    /// <summary>
    /// Index of the current advice being displayed.
    /// </summary>
    private int adviceId = 0;

    /// <summary>
    /// Initializes the tutorial UI and sets up event listeners.
    /// Activates the tutorial if the current scene is the tutorial scene.
    /// </summary>
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) ActiveTuto();
        continueButton.onClick.AddListener(ShowNextAdvice);
        watcher.primaryButtonPress.AddListener(OnPrimaryButtonEvent);
    }

    /// <summary>
    /// Handles primary button press events to activate the tutorial.
    /// </summary>
    /// <param name="pressed">Indicates whether the primary button was pressed.</param>
    private void OnPrimaryButtonEvent(bool pressed)
    {
        if (pressed)
        {
            ActiveTuto();
        }
    }

    /// <summary>
    /// Activates the tutorial by resetting the advice index and displaying the tutorial panel.
    /// </summary>
    public void ActiveTuto()
    {
        adviceId = 0; 
        TutorialPanel.SetActive(true);
        advices[adviceId].SetActive(true);
    }

    /// <summary>
    /// Displays the next advice in the tutorial. Hides the current advice and shows the next one.
    /// If all advices have been shown, hides the tutorial panel.
    /// </summary>
    private void ShowNextAdvice()
    {
        advices[adviceId].SetActive(false);
        adviceId++;
        if (advices.Length > adviceId)
        {
            advices[adviceId].SetActive(true);
        }
        else
        {
            TutorialPanel.SetActive(false);
        }
    }
}
