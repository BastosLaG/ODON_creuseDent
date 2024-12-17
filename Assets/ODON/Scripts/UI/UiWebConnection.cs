using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UiWebConnection : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI usernameText;
    [SerializeField] private TextMeshProUGUI passwordText;
    [SerializeField] private TextMeshProUGUI usernameIncorrectText;
    [SerializeField] private TextMeshProUGUI passwordIncorrectText;

    [SerializeField] private Button changeInterfaceButt;
    [SerializeField] private Button connectionButt;

    private bool connexionInterface = true;

    private void Start()
    {
        changeInterfaceButt.onClick.AddListener(Changeinterface);
        connectionButt.onClick.AddListener(TryToLogin);
        Web.OnUserLogin.AddListener(ReturnError);
    }

    private void TryToLogin()
    {
        StartCoroutine(Web.Login(usernameText.text, passwordText.text));
    }
    private void TryToRegister()
    {
        StartCoroutine(Web.Register(usernameText.text, passwordText.text));
    }

    private void ReturnError(string error)
    {
        if (error != null)
        {
            usernameIncorrectText.gameObject.SetActive(false);
            passwordIncorrectText.gameObject.SetActive(false);
            bool loginSuccess = true;
            if (error.Contains("Username"))
            {
                usernameIncorrectText.text = "Username does not exist";
                usernameIncorrectText.gameObject.SetActive(true);
                loginSuccess = false;
            }
            if (error.Contains("Password"))
            {
                passwordIncorrectText.text = "Password incorrect";
                passwordIncorrectText.gameObject.SetActive(true);
                loginSuccess = false;
            }
            if (loginSuccess && error.Contains("Login Success"))
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    private void Changeinterface()
    {
        connexionInterface = !connexionInterface;
        if (connexionInterface)
        {
            headerText.text = "Connexion";
            changeInterfaceButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "S'enregistrer";
            connectionButt.onClick.RemoveAllListeners();
            connectionButt.onClick.AddListener(TryToLogin);
        }
        else
        {
            headerText.text = "Enregistrement";
            changeInterfaceButt.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Se connecter";
            connectionButt.onClick.RemoveAllListeners();
            connectionButt.onClick.AddListener(TryToRegister);
        }

    }
}
