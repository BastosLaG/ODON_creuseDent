using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HighlightsTeethManager : MonoBehaviour
{
    [SerializeField] private TeethStruct[] teethStructList;
    [SerializeField] private int currentState;
    [SerializeField] private int maxState = 3;

    public int goodState = 0;
    public UnityEvent m_IsGoodStateEvent;
    public UnityEvent m_IsNotGoodStateEvent;

    [Header("Digue Settings")] 
    public Transform digueParent;
    [SerializeField] private GameObject[] digueList;

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

        if (m_IsGoodStateEvent == null)
            m_IsGoodStateEvent = new UnityEvent();
        if (m_IsNotGoodStateEvent == null)
            m_IsNotGoodStateEvent = new UnityEvent();
        
        List<PerçageDigue> allDigueInScene = new List<PerçageDigue>(
            FindObjectsByType<PerçageDigue>(FindObjectsSortMode.None)
        );

        foreach (PerçageDigue digue in allDigueInScene)
        {
            m_IsGoodStateEvent.AddListener(() => digue.SetIsTheGoodDigue(true));
            m_IsNotGoodStateEvent.AddListener(() => digue.SetIsTheGoodDigue(false));
        }


        currentState = 0;
        InitializeTeeth();
        InitializeDigue();
        CleanTeeth();
        CleanDigue();
        switchState(0);
    }

    private void InitializeTeeth()
    {
        int childCount = transform.childCount;
        teethStructList = new TeethStruct[childCount];

        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            TeethStruct toothStruct = new TeethStruct
            {
                tooth = child.gameObject,
                index = i % 8 + 1,
                state = (i <= 7) ? stateTheeth.UPPERRIGHT :
                        (i <= 15) ? stateTheeth.UPPERLEFT :
                        (i <= 23) ? stateTheeth.LOWERLEFT :
                                    stateTheeth.LOWERRIGHT
            };

            teethStructList[i] = toothStruct;
        }
    }

    private void InitializeDigue()
    {
        int childCount = digueParent.childCount;
        digueList = new GameObject[childCount];

        for (int i = 0; i < childCount; i++)
        {
            digueList[i] = digueParent.GetChild(i).gameObject;
        }

        if (digueList.Length != maxState)
        {
            Debug.LogError("Digue list length is not equal to maxState. Please check your settings.");
        }
        else 
        {
            Debug.Log("Digue list initialized correctly.");
        }
    }

    private void SetTeeth(stateTheeth state, int[] indexList)
    {
        foreach (var item in teethStructList)
        {
            if (item.state == state && Array.Exists(indexList, index => index == item.index))
            {
                item.tooth.SetActive(true);
            }
        }
    }

    private void SetDigue(int index)
    {
        if (index >= 0 && index < digueList.Length)
        {
            digueList[index].SetActive(true);
        }
    }

    public void switchState(int increment)
    {
        currentState = (currentState + increment + maxState) % maxState;

        CleanTeeth();
        CleanDigue();

        // set at true PerçageDigue.isGoodDigue
        if (currentState == goodState)
        {
            m_IsGoodStateEvent?.Invoke();
        }
        else
        {
            m_IsNotGoodStateEvent?.Invoke();
        }

        switch (currentState)
        {
            case 0:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 1 });
                break;
            case 1:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 3 });
                break;
            case 2:
                SetTeeth(stateTheeth.LOWERRIGHT, new int[] { 7 });
                break;
        }
        SetDigue(currentState);
    }

    private void CleanTeeth()
    {
        for (int i = 0; i < teethStructList.Length; i++)
        {
            teethStructList[i].tooth.SetActive(false);
        }
    }

    private void CleanDigue()
    {
        foreach (GameObject digue in digueList)
        {
            digue.SetActive(false);
        }
    }
}
