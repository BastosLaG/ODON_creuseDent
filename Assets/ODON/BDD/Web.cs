using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Events;

public class Web : MonoBehaviour
{
    private string webAddress = "http://localhost/odonbdd/Login.php";
    public UnityEvent<string> OnReturnedError;


    public void TryConnection(string username, string password)
    {
        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using UnityWebRequest www = UnityWebRequest.Post(webAddress, form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnReturnedError.Invoke(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
        }
    }
}
