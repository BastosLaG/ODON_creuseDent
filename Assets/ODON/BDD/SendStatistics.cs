using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SendStatistics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI etapeText;
    [SerializeField] private Transform errorLogParent;
    [SerializeField] private GameObject errorLogPref;
    [SerializeField] private string[][] etapeNames = new string[][] { 
        new string[] {
            "Étape 1 : Analysez le patient (la dent à soigner + allergie).",
            "Étape 2 : Equipez vous des équipements obligatoires (Gants, Masque et Blouse).",
            "Étape 3 : Dessinez sur la digue.",
            "Étape 4 : Percez la digue.",
            "Étape 5 : Posez le crampon sur la dent grâce à la pince de Brewer.",
            "Étape 6 : Posez la digue en faisant attention de bien passer derrière les ailettes.",
            "Étape 7 : Corrigez a l’aide du fil dentaire et posez le widget." 
        },
        new string[] {
            "Étape 1 : Analysez le patient(la dent à soigner + allergie).",
            "Étape 2 : Equipez vous des équipements obligatoires (Gants, Masque et Blouse).",
            "Étape 3 : Dessinez sur la digue.",
            "Étape 4 : Percez la digue.",
            "Étape 5 : Posez le crampon sur la digue.",
            "Étape 6 : Posez la digue avec le crampon.",
            "Étape 7 : Corrigez a l’aide du fil dentaire et posez widget."
        }
    };

    private int poseNum, etapeNum = 0;
    private Dictionary<string, int> etapes = new ();

    private void Start()
    {
        CreateNewPose(0);
        Web.OnDamInstallCreated.AddListener(CreateSteps);
        Web.OnInstallStepCreated.AddListener(AddEtape);
    }

    //ON CREE LES DONNÉES D'INSTALATION DE LA DIGUE ET DES ÉTAPES QUI LA COMPOSE
    private void CreateNewPose(int poseValue)
    {
        poseNum = poseValue;
        string poseName = poseValue == 0 ? "Pose crampon d'abord" : "Pose en parachute";

        // ENVOIE DES DONNÉES INSTALL DIGUE
        StartCoroutine(Web.NewDamInstall(poseName));
    }

    private void CreateSteps(string data)
    {
        // ENVOIE DES DONNÉES DES ETAPES D'INSTALL
        foreach (var etapeName in etapeNames[poseNum])
        {
            if (etapeName.Length > 100) Debug.LogError("Nom de l'étape trop long : " + etapeName.Length + " caractères / 100");
            StartCoroutine(Web.InstallStep(etapeName));
        }
        // AFFICHE L'ÉTAPE COURANTE
        showEtape();
    }

    // A CHAQUE ÉTAPE CRÉÉE, ON L'ENREGISTRE DANS UN DICTIONNAIRE POUR POUVOIR LUI AJOUTER SON TEMPS D'EXECUTION LE NOMBRE DE FOIS EXECUTÉE ET SES ERREURS SI BESOIN.
    private void AddEtape(string data)
    {
        if (data.Contains("Success"))
        {
            int id = int.Parse(data.Split("Success : ", StringSplitOptions.None)[1]);
            etapes[etapeNames[poseNum][etapeNum]] = id;
            etapeNum++;
        }
        if (etapeNum == etapeNames[poseNum].Length-1)
        {
            etapeNum = 0;
            CreateError("Mauvaise lecture du document");
            CreateError("Mauvaise interprétation du document");
            CreateError("Mauvaise selection de pose de digue");
        }
    }

    // AFFICHE L'ÉTAPE EN COURS À L'UTILISATEUR
    private void showEtape()
    {
        etapeText.text = etapeNames[poseNum][etapeNum];
    }

    // VA À L'ÉTAPE SUIVANTE
    public void NextEtape()
    {
        etapeNum++;
        showEtape();
    }

    // CRÉÉ UNE ERREUR À L'ÉTAPE ACTUEL
    public void CreateError(string errorName)
    {
        StartCoroutine(Web.StepMistake(errorName, etapes[etapeNames[poseNum][etapeNum]]));

        GameObject messageObj = Instantiate(errorLogPref, errorLogParent);
        messageObj.transform.SetSiblingIndex(0);
        messageObj.GetComponent<TextMeshProUGUI>().text = errorName;
    }

}
