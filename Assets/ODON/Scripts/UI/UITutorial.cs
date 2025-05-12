using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    public PrimaryButtonWatcher watcher;
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject[] advices;
    [SerializeField] private Button continueButton;
    private int adviceId = 0;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 2) ActiveTuto();
        continueButton.onClick.AddListener(ShowNextAdvice);
        watcher.primaryButtonPress.AddListener(OnPrimaryButtonEvent);
    }

    private void OnPrimaryButtonEvent(bool pressed)
    {
        if (pressed)
        {
            ActiveTuto();
        }
    }

    public void ActiveTuto()
    {
        adviceId = 0; 
        TutorialPanel.SetActive(true);
        advices[adviceId].SetActive(true);
    }

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
