using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiWebConnection : MonoBehaviour
{
    [SerializeField] private Web bddWeb;
    [SerializeField] private Text usernameText;
    [SerializeField] private Text passwordText;
    [SerializeField] private Text usernameIncorrectText;
    [SerializeField] private Text passwordIncorrectText;

    [SerializeField] private Button connectionButt;


    private void Start()
    {
        connectionButt.onClick.AddListener(TryToLogin);
        bddWeb.OnUserLogin.AddListener(ReturnError);
    }

    private void TryToLogin()
    {
        bddWeb.TryConnection(usernameText.text, passwordText.text);
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
}
