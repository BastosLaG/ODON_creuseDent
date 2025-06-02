using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class SceneLoader : MonoBehaviour
{
    public GameObject loadingScreen;
    public Image progressBar;
    public TextMeshProUGUI progressText;

    public Toggle m_Toggle;

    private string sceneGameName = "ODON1";
    private string sceneTutorialName = "Tutorial";

    private bool isTutorial = false;
    public string sceneName
    {
        get { return isTutorial ? sceneTutorialName : sceneGameName; }
        set { sceneGameName = value; }
    }

    void Start()
    {
        m_Toggle.onValueChanged.AddListener(delegate
        {
            ToggleValueChanged(m_Toggle);
        });
    }


    public void LoadScene()
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void LoadScene(string sceneName)
    {
        this.sceneName = sceneName;
        StartCoroutine(LoadSceneAsync(sceneName));
    }

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
    
    public void ToggleValueChanged(Toggle change)
    {
        isTutorial = change.isOn;
    }
}
