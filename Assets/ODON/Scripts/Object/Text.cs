using UnityEngine;
using TMPro;
using System;

public class calpText : MonoBehaviour
{
    public TMP_Text infoPatient;
    string patient1 = "Patient 1";
    string patient2 = "Patient 2";
    public int pose;


    public void UpdateTextboxes(int p){
        pose = p;
        print(pose);
        if (pose == 0){
            infoPatient.text = patient1;
        }else{
            infoPatient.text = patient2;
        }
        
    }
}
