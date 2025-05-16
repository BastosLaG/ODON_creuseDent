using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SendStatistics : MonoBehaviour
{
    //==========================
    // 🔧 SERIALIZED FIELDS
    //==========================
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI etapeText;
    [SerializeField] private calpText calpText;
    [SerializeField] private Transform errorLogParent;
    [SerializeField] private GameObject errorLogPref;

    [Header("Etapes")]
    [SerializeField] private string[][] etapeNames = new string[][]
    {
        new string[]
        {
            "Étape 1 : Analysez le patient (la dent à soigner + allergie).",
            "Étape 2 : Équipez-vous des équipements obligatoires (Gants, Masque et Blouse).",
            "Étape 3 : Percez la digue.",
            "Étape 4 : Posez le crampon sur la dent grâce à la pince de Brewer.",
            "Étape 5 : Posez la digue en faisant attention de bien passer derrière les ailettes.",
            "Étape 6 : Corrigez à l’aide du fil dentaire et posez le widget."
        },
        new string[]
        {
            "Étape 1 : Analysez le patient (la dent à soigner + allergie).",
            "Étape 2 : Équipez-vous des équipements obligatoires (Gants, Masque et Blouse).",
            "Étape 3 : Percez la digue.",
            "Étape 4 : Posez le crampon sur la digue.",
            "Étape 5 : Posez la digue avec le crampon.",
            "Étape 6 : Corrigez à l’aide du fil dentaire et posez le widget."
        }
    };
    [SerializeField] private List<float> etapeTimes = new();

    //==========================
    // 📦 PRIVATE FIELDS
    //==========================
    private int etapeNum = 0;
    private int poseNum;
    private DateTime damInstallTime;
    private DateTime stepTime;
    private readonly Dictionary<string, int> etapes = new();

    //==========================
    // ▶️ UNITY METHODS
    //==========================
    private void Start()
    {
        // Assigne une pose de digue aléatoire au démarrage
        CreateNewPose(UnityEngine.Random.Range(0, 2));
    }

    //==========================
    // 🛠 INITIALIZATION
    //==========================
    public void CreateNewPose(int poseValue)
    {
        poseNum = poseValue;
        calpText.UpdateTextboxes(poseNum);

        string poseName = poseNum == 0 ? "Pose crampon d'abord" : "Pose en parachute";
        damInstallTime = DateTime.Now;

        CreateSteps(); // Optionnel : si tu veux lancer automatiquement l’étape 1
    }

    private void CreateSteps()
    {
        stepTime = DateTime.Now;
        ShowEtape();
    }

    //==========================
    // 📘 ETAPE HANDLING
    //==========================
    public void NextEtape()
    {
        EtapeEnded();

        etapeNum++;

        if (etapeNum < etapeNames[poseNum].Length)
        {
            ShowEtape();
        }
        else
        {
            Debug.Log("Toutes les étapes complétées !");
        }
    }

    private void ShowEtape()
    {
        etapeText.text = etapeNames[poseNum][etapeNum];
        ODON.Manager_outline.Instance.UpdateOutline(ODON.Manager_outline.ItemType.Tablet); // Adaptable
    }

    private void EtapeEnded()
    {
        DateTime now = DateTime.Now;
        etapeTimes.Add((float)(now - stepTime).TotalSeconds);
        stepTime = now;
    }
}
