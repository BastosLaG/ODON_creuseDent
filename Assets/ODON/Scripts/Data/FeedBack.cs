using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class Feedback : MonoBehaviour
{

    [SerializeField] private string formUrl = "https://docs.google.com/forms/u/0/d/e/1FAIpQLSeuA82XVzL6SUEzNIhZAp3_nYok95jLcAybFuUT6QJwBztRdw/formResponse";
    [SerializeField] private List<string> entries = new();
    private Dictionary<int, string> datas = new();

    private string[] postEntries;


    private void Start()
    {
        for (int i = 0; i < entries.Count; i++)
        {
            datas.Add(i, "");
            // print(i + " " + datas[i]);
        }
    }

    public void AssignData(int entrieId, string data)
    {
        datas[entrieId] = data;
    }


    public void SubmitFeedback()
    {

        foreach (int dataId in datas.Keys)
        {
            if (datas[dataId].Length < 1)
            {
                Debug.LogError("Entries numbers doesn't correspond to data number");
                return;
            }
        }
        postEntries = entries.ToArray();
        string[] postDatas = datas.Values.ToArray();
        StartCoroutine(Post(postDatas));
    }


    private IEnumerator Post(string[] postDatas)
    {
        WWWForm form = new WWWForm();
        for (int i = 0; i < postEntries.Length; i++)
        {
            form.AddField("entry." + entries[i], postDatas[i]);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(formUrl, form))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Feedback submitted successfully.");
            }
            else
            {
                Debug.LogError("Error in feedback submission: " + www.error);
            }
        }
    }
}