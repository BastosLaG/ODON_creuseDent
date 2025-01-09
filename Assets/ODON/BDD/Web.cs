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

    //CREATE THE NEW DAM INSTALL
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
            if (data.Contains("Success"))
            {
                int id = int.Parse(data.Split("Success : ", StringSplitOptions.None)[1]);
                PlayerPrefs.SetInt("installID", id);
                OnDamInstallCreated.Invoke(data);
            }
        }
    }

    //UPDATE THE NEW DAM INSTALL
    public static IEnumerator UpdateDamInstall(string installTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("installID", PlayerPrefs.GetInt("installID"));
        form.AddField("CompletTime", installTime);
        form.AddField("user", PlayerPrefs.GetInt("UserID"));

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "PoseUpdate.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log("UpdateDamInstall : " + data);
        }
    }

    // CREATE INSTALL STEP FOR THE DAMINSTALL
    public static IEnumerator InstallStep(string stepname)
    {
        WWWForm form = new WWWForm();
        form.AddField("StepName", stepname);
        form.AddField("dentalDamInstall", PlayerPrefs.GetInt("installID"));

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
        }
    }

    // UPDATE STEP OF DAMINSTALL
    public static IEnumerator UpdateStep(string stepname, string stepTime)
    {
        WWWForm form = new WWWForm();
        form.AddField("StepName", stepname);
        form.AddField("dentalDamInstall", PlayerPrefs.GetInt("installID"));
        form.AddField("StepCompletTime", stepTime);

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "InstallUpdate.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            string data = www.downloadHandler.text;
            Debug.Log("Updatestep : " + data);
        }
    }

    // CREATE NEW MISTAKE WITH NAME AND THE INSTALLDAM KEY
    public static IEnumerator StepMistake(string mistakeName, int installId)
    {

        WWWForm form = new WWWForm();
        form.AddField("mistakeName", mistakeName);
        form.AddField("installID", PlayerPrefs.GetInt("installID"));

        using UnityWebRequest www = UnityWebRequest.Post(webAddress + "Mistake.php", form);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            OnStepMistakeCreated.Invoke(www.downloadHandler.text);
            //Debug.Log(www.downloadHandler.text);
        }
    }
}
