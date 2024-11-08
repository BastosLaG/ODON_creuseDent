using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Events;
using System;

public class Web : MonoBehaviour
{
    private string webAddress = "http://localhost/odonbdd/";
    public UnityEvent<string> OnUserLogin;
    public UnityEvent<string> OnDamInstallCrated;


    public void Start()
    {
        PlayerPrefs.SetInt("UserID", 1);
        StartCoroutine(NewDamInstall("Pose Test 1"));
    }

    public void TryConnection(string username, string password)
    {
        StartCoroutine(Login(username, password));
    }

    IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Login.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            OnUserLogin.Invoke(data);
            Debug.Log(data);
            if (data.Contains("Login Success"))
            {
                int id = int.Parse(data.Split("Login Success", StringSplitOptions.None)[1]);
                PlayerPrefs.SetInt("UserID", id);
            }
        }
    }

    IEnumerator NewDamInstall(string installname)
    {
        WWWForm form = new WWWForm();
        form.AddField("poseName", installname);
        form.AddField("poseUser", PlayerPrefs.GetInt("UserID"));

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Pose.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnDamInstallCrated.Invoke(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
        }
    }
}
