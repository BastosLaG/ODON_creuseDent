using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UITutorial : MonoBehaviour
{
    [SerializeField] private GameObject TutorialPanel;
    [SerializeField] private GameObject[] advices;
    [SerializeField] private Button continueButton;
    private int adviceId = 0;

    private void Start()
    {
        TutorialPanel.SetActive(true);
        continueButton.onClick.AddListener(ShowNextAdvice);
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
