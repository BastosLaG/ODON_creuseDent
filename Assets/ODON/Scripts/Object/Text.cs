using UnityEngine;
using TMPro;
using System;

public class calpText : MonoBehaviour
{
    public TMP_Text infoPatient;
    string patient1 = "Nom : Dubois\nPrénom : Michel\nAllergie Latex : Non\n Dent : 41\n";
    string patient2 = "Nom : Dubois\nPrénom : Michel\nAllergie Latex : Non\n Dent : 41\n";


    public void UpdateTextboxes(int p){
        if (p == 0){
            infoPatient.text = patient1;
        }else{
            infoPatient.text = patient2;
        }
        
    }
}
