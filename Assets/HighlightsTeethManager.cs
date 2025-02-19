using UnityEngine;
using System.Collections.Generic;

public class HighlightsTeethManager : MonoBehaviour
{
    [SerializeField] private GameObject[] teethSphere;
    [SerializeField] private Dictionary<stateTheeth, List<GameObject>> teethGroups = new();

    private enum stateTheeth
    {
        UPPERRIGHT,
        UPPERLEFT,
        LOWERLEFT,
        LOWERRIGHT
    };

    void Start()
    {
        InitializeTeeth();
        SortTeeth();
        CleanTeeth();
    }

    void InitializeTeeth()
    {
        Transform[] children = GetComponentsInChildren<Transform>();
        teethSphere = new GameObject[children.Length - 1];

        int i = 0;
        foreach (Transform child in children)
        {
            if (child != transform)
            {
                teethSphere[i] = child.gameObject;
                i++;
            }
        }

        // Initialiser les groupes
        foreach (stateTheeth state in System.Enum.GetValues(typeof(stateTheeth)))
        {
            teethGroups[state] = new List<GameObject>();
        }
    }

    void SortTeeth()
    {
        int i = 0;
        foreach (GameObject tooth in teethSphere)
        {
            if (i <= 7 ) {
                teethGroups[stateTheeth.UPPERRIGHT].Add(tooth);
            } else if (i > 7 && i <= 15) {
                teethGroups[stateTheeth.UPPERLEFT].Add(tooth); 
            } else if (i > 15 && i <= 23){
                teethGroups[stateTheeth.LOWERLEFT].Add(tooth);
            } else if (i > 23 && i <= 31) {
                teethGroups[stateTheeth.LOWERRIGHT].Add(tooth);
            }
            i++;
        }

        // Vérification dans la console
        foreach (var group in teethGroups)
        {
            Debug.Log(group.Key + " : " + group.Value.Count + " teeth");
        }
    }

    void CleanTeeth()
    {
        foreach (GameObject tooth in teethSphere)
        {
            tooth.SetActive(false);
        }
    }
}
