using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.Events;
using System;

public static class Web
{
    private static string webAddress = "http://localhost/odonbdd/";
    public static UnityEvent<string> OnUserLogin = new();
    public static UnityEvent<string> OnDamInstallCreated = new();
    public static UnityEvent<string> OnInstallStepCreated = new();
    public static UnityEvent<string> OnStepMistakeCreated = new();

    public static IEnumerator Login(string username, string password)
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
                NewDamInstall("Installation d'une digue");
            }
        }
    }

    public static IEnumerator Register(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Register.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            OnUserLogin.Invoke(data);
            Debug.LogError(data);
        }
    }

    public static IEnumerator NewDamInstall(string installname)
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
            string data = www.downloadHandler.text;
            OnDamInstallCreated.Invoke(data);
            Debug.Log(data);
            if (data.Contains("Success"))
            {
                int id = int.Parse(data.Split("Success", StringSplitOptions.None)[1]);
                PlayerPrefs.SetInt("installID", id);
            }
        }
    }

    public static IEnumerator InstallStep(string stepname)
    {
        WWWForm form = new WWWForm();
        form.AddField("stepName", stepname);
        form.AddField("dentalDamInstall", PlayerPrefs.GetInt("dentalDamID"));

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Install.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            OnInstallStepCreated.Invoke(data);
            Debug.Log(data);
            if (data.Contains("Success"))
            {
                int id = int.Parse(data.Split("Success", StringSplitOptions.None)[1]);
                PlayerPrefs.SetInt("stepID", id);
            }
        }
    }

    public static IEnumerator StepMistake(string mistakeName)
    {

        WWWForm form = new WWWForm();
        form.AddField("mistakeName", mistakeName);
        form.AddField("InstallStep", PlayerPrefs.GetInt("InstallStepID"));

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Mistake.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnStepMistakeCreated.Invoke(www.downloadHandler.text);
            Debug.Log(www.downloadHandler.text);
        }
    }
}
