using System;
using Unity.VisualScripting;
using UnityEngine;

public class HighlightsTeethManager : MonoBehaviour
{
    [SerializeField] private TeethStruct[] teethStructList;
    [SerializeField] private int currentState;
    [SerializeField] private int maxState;
    public enum stateTheeth
    {
        UPPERRIGHT,
        UPPERLEFT,
        LOWERLEFT,
        LOWERRIGHT
    };

    [System.Serializable]
    public struct TeethStruct
    {
        public stateTheeth state;
        public GameObject tooth;
        public int index;
    }

    void Start()
    {
        currentState = 0;
        maxState = 3;
        InitializeTeeth();
        CleanTeeth();
        switchState(0);
    }


    void InitializeTeeth()
    {
        Transform[] children = GetComponentsInChildren<Transform>();

        // Initialiser la taille du tableau teethStructList
        teethStructList = new TeethStruct[children.Length - 1];

        int i = 0;
        foreach (Transform child in children)
        {
            if (child != transform)
            {
                TeethStruct toothStruct = new TeethStruct();
                toothStruct.tooth = child.gameObject;
                toothStruct.index = i%8+1;

                if (i <= 7)
                {
                    toothStruct.state = stateTheeth.UPPERRIGHT;
                }
                else if (i > 7 && i <= 15)
                {
                    toothStruct.state = stateTheeth.UPPERLEFT;
                }
                else if (i > 15 && i <= 23)
                {
                    toothStruct.state = stateTheeth.LOWERLEFT;
                }
                else if (i > 23 && i <= 31)
                {
                    toothStruct.state = stateTheeth.LOWERRIGHT;
                }

                teethStructList[i] = toothStruct;
                i++;
            }
        }
    }
    void SetTeeth(stateTheeth state, int[] indexList)
    {
        foreach (TeethStruct item in teethStructList)
        {
            if (item.state == state)
            {
                foreach (int index in indexList)
                {
                    if (item.index == index)
                    {
                        item.tooth.SetActive(true);
                    }
                }
            }
        }
}

    public void switchState(int increment){
        currentState = (currentState+increment)%maxState;
        CleanTeeth();
        switch (currentState) {
            case 0:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 7 });
                break;    
            case 1:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 3 });
                break;
            case 2:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 1 });          
                break;
        }
    }

    void CleanTeeth()
    {
        foreach (TeethStruct tooth in teethStructList)
        {
            tooth.tooth.SetActive(false);
        }
    }
}
