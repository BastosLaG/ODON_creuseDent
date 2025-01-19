using UnityEngine;
using TMPro;

public class Text : MonoBehaviour
{
    public TMP_Text infoPatient;
    string info = "Patient 1";

    void Start(){
        UpdateTextboxes();
    }


    void UpdateTextboxes(){
        infoPatient.text = info;
    }
}
