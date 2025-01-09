using System;
using System.Collections;
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

    [SerializeField] private DateTime daminstallTime, stepTime;
    private int poseNum, etapeNum = 0;
    private Dictionary<string, int> etapes = new ();

    private void Start()
    {
        // APPEL DE LA FONCTION DE TEST
        StartCoroutine("BDDTest");
    }


    private IEnumerator BDDTest()
    {
        print("creation de la pose et de ses étapes et erreurs.");
        // CETTE FONCTION SERA APPELLÉ AUX CHOIX DE LA POSE DE DIGUE.
        CreateNewPose(0);
        for (int i = 0; i < etapeNames[poseNum].Length; i++)
        {
            int rand = UnityEngine.Random.Range(1, 5);
            yield return new WaitForSeconds(rand);
            print("Etape" + i + "accomplie en" + rand + "secondes.");
            // CETTE FONCTION SERA APPELLÉ AU CHAQUE FOIS QU'UNE ÉTAPE AURA ÉTÉ COMPLÉTÉE.
            NextEtape();
        }

    }


    // ON CREE LES DONNÉES D'INSTALATION DE LA DIGUE ET DES ÉTAPES QUI LA COMPOSE
    public void CreateNewPose(int poseValue)
    {
        poseNum = poseValue;
        string poseName = poseValue == 0 ? "Pose crampon d'abord" : "Pose en parachute";

        // ENVOIE DES DONNÉES INSTALL DIGUE
        StartCoroutine(Web.NewDamInstall(poseName));
        daminstallTime = DateTime.Now;

        // LANCE LA CREATION DES ÉTAPES
        Web.OnDamInstallCreated.AddListener(CreateSteps);
        Web.OnInstallStepCreated.AddListener(AddEtape);
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
        stepTime = DateTime.Now;
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

    private void etapeEnded()
    {
        StartCoroutine(Web.UpdateStep(etapeNames[poseNum][etapeNum], (DateTime.Now - stepTime).ToString("hh\\:mm\\:ss")));
        stepTime = DateTime.Now;
    }

    // AFFICHE L'ÉTAPE EN COURS À L'UTILISATEUR
    private void showEtape()
    {
        etapeText.text = etapeNames[poseNum][etapeNum];
    }

    // VA À L'ÉTAPE SUIVANTE
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
            Debug.Log("toutes les étapes complétées !");
        }
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
