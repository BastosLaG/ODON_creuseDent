using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class SendStatistics : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI etapeText;
    [SerializeField] private calpText calpText;
    [SerializeField] private Transform errorLogParent;
    [SerializeField] private GameObject errorLogPref;
    [SerializeField] private string[][] etapeNames = new string[][] { 
        new string[] {
            "�tape 1 : Analysez le patient (la dent � soigner + allergie).",
            "�tape 2 : Equipez vous des �quipements obligatoires (Gants, Masque et Blouse).",
            "�tape 3 : Dessinez sur la digue.",
            "�tape 4 : Percez la digue.",
            "�tape 5 : Posez le crampon sur la dent gr�ce � la pince de Brewer.",
            "�tape 6 : Posez la digue en faisant attention de bien passer derri�re les ailettes.",
            "�tape 7 : Corrigez a l�aide du fil dentaire et posez le widget." 
        },
        new string[] {
            "�tape 1 : Analysez le patient(la dent � soigner + allergie).",
            "�tape 2 : Equipez vous des �quipements obligatoires (Gants, Masque et Blouse).",
            "�tape 3 : Dessinez sur la digue.",
            "�tape 4 : Percez la digue.",
            "�tape 5 : Posez le crampon sur la digue.",
            "�tape 6 : Posez la digue avec le crampon.",
            "�tape 7 : Corrigez a l�aide du fil dentaire et posez widget."
        }
    };

    [SerializeField] private DateTime daminstallTime, stepTime;
    private int etapeNum = 0;
    private int poseNum;
    private Dictionary<string, int> etapes = new ();

    
    private void Start()
    {
        // ASSIGNE UNE POSE DE DIGUE ALEATOIRE.
        CreateNewPose(UnityEngine.Random.Range(0,2));
    }

    // ON CREE LES DONN�ES D'INSTALATION DE LA DIGUE ET DES �TAPES QUI LA COMPOSE
    public void CreateNewPose(int poseValue)
    {
        poseNum = poseValue;
        calpText.UpdateTextboxes(poseNum);
        string poseName = poseValue == 0 ? "Pose crampon d'abord" : "Pose en parachute";

        // ENVOIE DES DONN�ES INSTALL DIGUE
        StartCoroutine(Web.NewDamInstall(poseName));
        daminstallTime = DateTime.Now;

        // LANCE LA CREATION DES �TAPES
        Web.OnDamInstallCreated.AddListener(CreateSteps);
        Web.OnInstallStepCreated.AddListener(AddEtape);
    }

    private void CreateSteps(string data)
    {
        // ENVOIE DES DONN�ES DES ETAPES D'INSTALL
        foreach (var etapeName in etapeNames[poseNum])
        {
            if (etapeName.Length > 100) Debug.LogError("Nom de l'�tape trop long : " + etapeName.Length + " caract�res / 100");
            StartCoroutine(Web.InstallStep(etapeName));
        }
        // AFFICHE L'�TAPE COURANTE
        showEtape();
        stepTime = DateTime.Now;
    }

    // A CHAQUE �TAPE CR��E, ON L'ENREGISTRE DANS UN DICTIONNAIRE POUR POUVOIR LUI AJOUTER SON TEMPS D'EXECUTION LE NOMBRE DE FOIS EXECUT�E ET SES ERREURS SI BESOIN.
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
            CreateError("Mauvaise interpr�tation du document");
            CreateError("Mauvaise selection de pose de digue");
        }
    }

    private void etapeEnded()
    {
        StartCoroutine(Web.UpdateStep(etapeNames[poseNum][etapeNum], (DateTime.Now - stepTime).ToString("hh\\:mm\\:ss")));
        stepTime = DateTime.Now;
    }

    // AFFICHE L'�TAPE EN COURS � L'UTILISATEUR
    private void showEtape()
    {
        etapeText.text = etapeNames[poseNum][etapeNum];
    }

    // VA � L'�TAPE SUIVANTE
    public void NextEtape()
    {
        etapeEnded();
        etapeNum++;
        if (etapeNum < etapeNames[poseNum].Length)
        {
            showEtape();
        }
        else
        {
            StartCoroutine(Web.UpdateDamInstall((DateTime.Now - daminstallTime).ToString("hh\\:mm\\:ss")));
            Debug.Log("toutes les �tapes compl�t�es !");
        }
    }

    // CR�� UNE ERREUR � L'�TAPE ACTUEL
    public void CreateError(string errorName)
    {
        StartCoroutine(Web.StepMistake(errorName, etapes[etapeNames[poseNum][etapeNum]]));

        GameObject messageObj = Instantiate(errorLogPref, errorLogParent);
        messageObj.transform.SetSiblingIndex(0);
        messageObj.GetComponent<TextMeshProUGUI>().text = errorName;
    }
}
